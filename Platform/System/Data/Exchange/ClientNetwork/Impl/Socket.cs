using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface Socket {
        int Available { get; }

        void Connect(string host, int port);

        void Connect(IPAddress host, int port);

        void Disconnect(bool b);

        Stream GetStream();

        bool Poll(int i, SelectMode selectMode);

        int Receive(byte[] socketBytes);

        void Close();
    }
}