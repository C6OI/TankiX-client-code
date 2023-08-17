using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class SocketWrapper : Socket {
        readonly global::System.Net.Sockets.Socket socket = new(AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);

        public int Available => socket.Available;

        public void Connect(string host, int port) => socket.Connect(host, port);

        public void Connect(IPAddress host, int port) => socket.Connect(host, port);

        public void Disconnect(bool b) => socket.Disconnect(b);

        public Stream GetStream() => new NetworkStream(socket);

        public bool Poll(int i, SelectMode selectMode) => socket.Poll(i, selectMode);

        public int Receive(byte[] socketBytes) => socket.Receive(socketBytes);

        public void Close() => socket.Close();
    }
}