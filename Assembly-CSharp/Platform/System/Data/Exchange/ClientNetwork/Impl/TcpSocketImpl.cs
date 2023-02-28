using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;
using Platform.Library.ClientLogger.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class TcpSocketImpl : PlatformSocket {
        const AddressFamily ADDRESS_FAMILY = AddressFamily.InterNetwork;

        Socket socket;

        public bool IsConnected => socket != null && socket.Connected;

        public int AvailableLength => socket != null ? socket.Available : 0;

        public bool CanRead => socket.Poll(0, SelectMode.SelectRead);

        public bool CanWrite => socket.Poll(0, SelectMode.SelectWrite);

        public void ConnectAsync(string host, int port, Action completeCallback) {
            if (socket != null && socket.Connected) {
                throw new Exception("Connection in progress");
            }

            IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
            IPAddress iPAddress = hostAddresses.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);

            if (iPAddress == null) {
                LogUnresolvableAddress(host, hostAddresses);
                throw new Exception("Unresolvable address");
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs socketAsyncEventArgs = new();
            socketAsyncEventArgs.RemoteEndPoint = new IPEndPoint(iPAddress, port);

            socketAsyncEventArgs.Completed += delegate(object sender, SocketAsyncEventArgs e) {
                if (e.SocketError != 0) {
                    socket = null;
                }

                completeCallback();
            };

            socket.ConnectAsync(socketAsyncEventArgs);
        }

        public int Read(byte[] buffer, int offset, int count) => socket.Receive(buffer, offset, count, SocketFlags.None);

        public void Write(byte[] buffer, int offset, int count) {
            socket.Send(buffer, offset, count, SocketFlags.None);
        }

        public void Disconnect() {
            socket.Close();
            socket = null;
        }

        void LogUnresolvableAddress(string host, IPAddress[] addressList) {
            ILog logger = LoggerProvider.GetLogger(this);
            logger.ErrorFormat("Couldn't resolve host address {0} as {1} family", host, AddressFamily.InterNetwork);
            logger.ErrorFormat("Available options (Count = {0}):", addressList.Length);

            foreach (IPAddress iPAddress in addressList) {
                logger.ErrorFormat("{0}, address familiy - {1}", iPAddress, iPAddress.AddressFamily);
            }
        }
    }
}