using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender {
    public class TelnetAppender : AppenderSkeleton {
        static readonly Type declaringType = typeof(TelnetAppender);

        SocketHandler m_handler;

        int m_listeningPort = 23;

        public int Port {
            get => m_listeningPort;
            set {
                if (value < 0 || value > 65535) {
                    throw SystemInfo.CreateArgumentOutOfRangeException("value",
                        value,
                        "The value specified for Port is less than " +
                        0.ToString(NumberFormatInfo.InvariantInfo) +
                        " or greater than " +
                        65535.ToString(NumberFormatInfo.InvariantInfo) +
                        ".");
                }

                m_listeningPort = value;
            }
        }

        protected override bool RequiresLayout => true;

        protected override void OnClose() {
            base.OnClose();

            if (m_handler != null) {
                m_handler.Dispose();
                m_handler = null;
            }
        }

        public override void ActivateOptions() {
            base.ActivateOptions();

            try {
                LogLog.Debug(declaringType, "Creating SocketHandler to listen on port [" + m_listeningPort + "]");
                m_handler = new SocketHandler(m_listeningPort);
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Failed to create SocketHandler", exception);
                throw;
            }
        }

        protected override void Append(LoggingEvent loggingEvent) {
            if (m_handler != null && m_handler.HasConnections) {
                m_handler.Send(RenderLoggingEvent(loggingEvent));
            }
        }

        protected class SocketHandler : IDisposable {
            const int MAX_CONNECTIONS = 20;

            ArrayList m_clients = new();

            Socket m_serverSocket;

            public SocketHandler(int port) {
                m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                m_serverSocket.Listen(5);
                m_serverSocket.BeginAccept(OnConnect, null);
            }

            public bool HasConnections {
                get {
                    ArrayList clients = m_clients;
                    return clients != null && clients.Count > 0;
                }
            }

            public void Dispose() {
                ArrayList clients = m_clients;

                foreach (SocketClient item in clients) {
                    item.Dispose();
                }

                m_clients.Clear();
                Socket serverSocket = m_serverSocket;
                m_serverSocket = null;

                try {
                    serverSocket.Shutdown(SocketShutdown.Both);
                } catch { }

                try {
                    serverSocket.Close();
                } catch { }
            }

            public void Send(string message) {
                ArrayList clients = m_clients;

                foreach (SocketClient item in clients) {
                    try {
                        item.Send(message);
                    } catch (Exception) {
                        item.Dispose();
                        RemoveClient(item);
                    }
                }
            }

            void AddClient(SocketClient client) {
                lock (this) {
                    ArrayList arrayList = (ArrayList)m_clients.Clone();
                    arrayList.Add(client);
                    m_clients = arrayList;
                }
            }

            void RemoveClient(SocketClient client) {
                lock (this) {
                    ArrayList arrayList = (ArrayList)m_clients.Clone();
                    arrayList.Remove(client);
                    m_clients = arrayList;
                }
            }

            void OnConnect(IAsyncResult asyncResult) {
                try {
                    Socket socket = m_serverSocket.EndAccept(asyncResult);
                    LogLog.Debug(declaringType, "Accepting connection from [" + socket.RemoteEndPoint + "]");
                    SocketClient socketClient = new(socket);
                    int count = m_clients.Count;

                    if (count < 20) {
                        try {
                            socketClient.Send("TelnetAppender v1.0 (" + (count + 1) + " active connections)\r\n\r\n");
                            AddClient(socketClient);
                            return;
                        } catch {
                            socketClient.Dispose();
                            return;
                        }
                    }

                    socketClient.Send("Sorry - Too many connections.\r\n");
                    socketClient.Dispose();
                } catch { } finally {
                    if (m_serverSocket != null) {
                        m_serverSocket.BeginAccept(OnConnect, null);
                    }
                }
            }

            protected class SocketClient : IDisposable {
                Socket m_socket;

                StreamWriter m_writer;

                public SocketClient(Socket socket) {
                    m_socket = socket;

                    try {
                        m_writer = new StreamWriter(new NetworkStream(socket));
                    } catch {
                        Dispose();
                        throw;
                    }
                }

                public void Dispose() {
                    try {
                        if (m_writer != null) {
                            m_writer.Close();
                            m_writer = null;
                        }
                    } catch { }

                    if (m_socket != null) {
                        try {
                            m_socket.Shutdown(SocketShutdown.Both);
                        } catch { }

                        try {
                            m_socket.Close();
                        } catch { }

                        m_socket = null;
                    }
                }

                public void Send(string message) {
                    m_writer.Write(message);
                    m_writer.Flush();
                }
            }
        }
    }
}