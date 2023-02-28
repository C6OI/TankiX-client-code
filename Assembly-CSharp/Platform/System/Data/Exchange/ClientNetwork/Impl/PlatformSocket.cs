using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface PlatformSocket {
        bool IsConnected { get; }

        int AvailableLength { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        void ConnectAsync(string host, int port, Action completeCallback);

        int Read(byte[] buffer, int offset, int count);

        void Write(byte[] buffer, int offset, int count);

        void Disconnect();
    }
}