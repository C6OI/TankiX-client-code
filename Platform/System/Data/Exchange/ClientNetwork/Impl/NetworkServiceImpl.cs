using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using log4net;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientProtocol.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class NetworkServiceImpl : NetworkService {
        const int BUFFER_SIZE = 51200;

        readonly ILog log;

        readonly Queue<CommandPacket> packetQueue = new();

        readonly ProtocolAdapter protocolAdapter;

        readonly Socket socket;

        volatile bool connected;

        byte[] readBuffer = new byte[51200];

        Stream socketStream;

        public NetworkServiceImpl(Socket socket, ProtocolAdapter protocolAdapter) {
            log = LoggerProvider.GetLogger(this);
            this.protocolAdapter = protocolAdapter;
            this.socket = socket;
        }

        [Inject] public static ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }

        [Inject] public static ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }

        public bool RethrowExceptions { get; set; }

        public bool Connected => connected;

        public event Action<CommandPacket> OnCommandPacketReceived;

        public void Connect(string host, int[] ports) {
            if (connected) {
                log.Warn("Opening already connected socket.");
                return;
            }

            foreach (int port in ports) {
                if (connected |= TryConnect(host, port)) {
                    break;
                }
            }

            if (!connected) {
                throw new CannotConnectException(host, ports);
            }

            socketStream = socket.GetStream();
        }

        public void ProcessNetworkTasks() {
            if (connected) {
                if (PollSocket(SelectMode.SelectRead)) {
                    ProcessSelectRead();
                }

                if (connected && PollSocket(SelectMode.SelectWrite)) {
                    WriteCommands();
                }
            }
        }

        public void SendCommandPacket(CommandPacket packet) => packetQueue.Enqueue(packet);

        public void Disconnect() => Disconnect(ProblemStatus.ClosedByClient);

        protected virtual Socket CreateSocket() => new SocketWrapper();

        bool TryConnect(string host, int port) {
            try {
                socket.Connect(host, port);
            } catch (SocketException exception) {
                log.Fatal(string.Format("Connect to {0}:{1}", host, port), exception);
                return false;
            }

            return true;
        }

        bool PollSocket(SelectMode selectMode) {
            try {
                return socket.Poll(0, selectMode);
            } catch (SocketException e) {
                OnSocketProblem(ProblemStatus.SocketMethodInvokeError, e);
                return false;
            }
        }

        void ProcessSelectRead() {
            try {
                if (socket.Available == 0) {
                    Disconnect(ProblemStatus.ClosedByServer);
                } else {
                    OnDataReceived();
                }
            } catch (SocketException e) {
                OnSocketProblem(ProblemStatus.SocketMethodInvokeError, e);
            }
        }

        void OnDataReceived() {
            int num;

            try {
                int available = socket.Available;
                readBuffer = BufferUtils.GetBufferWithValidSize(readBuffer, available);
                num = socket.Receive(readBuffer);
                log.InfoFormat("Received {0} byte(s).", num);
            } catch (SocketException e) {
                OnSocketProblem(ProblemStatus.ReceiveError, e);
                return;
            }

            try {
                log.Info("Processing packet.");
                protocolAdapter.AddChunk(readBuffer, num);
                CommandPacket packet;

                while (protocolAdapter.TryDecode(out packet)) {
                    if (OnCommandPacketReceived != null) {
                        OnCommandPacketReceived(packet);
                    }

                    protocolAdapter.FinalizeDecodedCommandPacket(packet);
                }
            } catch (Exception e2) {
                OnSocketProblem(ProblemStatus.DecodePacketError, e2);
            }
        }

        void WriteCommands() {
            while (packetQueue.Count != 0) {
                CommandPacket commandPacket = packetQueue.Dequeue();
                MemoryStreamData memoryStreamData = null;

                try {
                    memoryStreamData = protocolAdapter.Encode(commandPacket);
                } catch (Exception e) {
                    OnSocketProblem(ProblemStatus.EncodeError, e);
                    break;
                } finally {
                    ClientNetworkInstancesCache.ReleaseCommandPacketWithCommandsCollection(commandPacket);
                }

                try {
                    log.InfoFormat("Write {0} byte(s).", memoryStreamData.Length);
                    byte[] bytes = Encoding.UTF8.GetBytes("CONNECT");
                    bool flag = true;

                    for (int i = 0; i < bytes.Length; i++) {
                        if (bytes[i] != memoryStreamData.GetBuffer()[0]) {
                            flag = false;
                            break;
                        }
                    }

                    if (flag) {
                        log.WarnFormat("Write ProtocolCorrupted {0}",
                            string.Join(", ", commandPacket.Commands.Select(c => c.ToString()).ToArray()));

                        log.WarnFormat("Write ProtocolCorrupted bytes {0}", BitConverter.ToString(bytes));
                    }

                    socketStream.Write(memoryStreamData.GetBuffer(), 0, (int)memoryStreamData.Length);
                } catch (Exception e2) {
                    OnSocketProblem(ProblemStatus.SendError, e2);
                } finally {
                    ClientProtocolInstancesCache.ReleaseMemoryStreamData(memoryStreamData);
                }
            }
        }

        void OnSocketProblem(ProblemStatus status, Exception e) {
            if (RethrowExceptions) {
                throw new NetworkException("OnSocketProblem " + status, e);
            }

            log.Fatal("Server command error: " + GetMessage(e), e);
            DisconnectOnProblemIfNeeded(status);
        }

        static string GetMessage(Exception e) {
            if (e is TargetInvocationException && e.InnerException != null) {
                return e.InnerException.GetType().Name + ": " + e.InnerException.Message;
            }

            return e.Message;
        }

        void DisconnectOnProblemIfNeeded(ProblemStatus status) {
            switch (status) {
                case ProblemStatus.SocketMethodInvokeError:
                case ProblemStatus.ReceiveError:
                case ProblemStatus.SendError:
                    Disconnect(status);
                    break;

                case ProblemStatus.DecodePacketError:
                case ProblemStatus.DecodeCommandError:
                case ProblemStatus.EncodeError:
                    break;
            }
        }

        public void SendCommand(Command command) {
            List<Command> commandCollection = ClientNetworkInstancesCache.GetCommandCollection();
            commandCollection.Add(command);
            CommandPacket commandPacketInstance = ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);
            SendCommandPacket(commandPacketInstance);
        }

        protected void Disconnect(ProblemStatus status) {
            log.InfoFormat("Disconnect by {0}.", status);

            if (!connected) {
                log.Warn("Closing not connected socket.");
                return;
            }

            connected = false;

            try {
                socket.Disconnect(false);
                socket.Close();
            } catch (SocketException exception) {
                log.Fatal("Disconnect error.", exception);
            }
        }
    }
}