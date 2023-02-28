using System;
using System.IO;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class WebSocketImpl : PlatformSocket {
        Action completeCallback;
        WebSocket socket;

        public bool IsConnected => socket != null && socket.IsConnected;

        public int AvailableLength => socket != null ? socket.AvailableLength : 0;

        public bool CanRead => AvailableLength > 0;

        public bool CanWrite => IsConnected;

        public void ConnectAsync(string host, int port, Action completeCallback) {
            if (socket != null && socket.IsConnected) {
                throw new Exception("Connection in progress");
            }

            this.completeCallback = completeCallback;
            socket = new WebSocket();
            socket.ConnectAsync(string.Format("ws://{0}:{1}/websocket", host, port), OnComplete);
        }

        public int Read(byte[] buffer, int offset, int count) {
            int result = socket.Receive(buffer);

            if (socket.GetError() != null) {
                throw new IOException(socket.GetError());
            }

            return result;
        }

        public void Write(byte[] buffer, int offset, int count) {
            byte[] array = new byte[count];
            Buffer.BlockCopy(buffer, 0, array, 0, count);
            socket.Send(array);

            if (socket.GetError() != null) {
                throw new IOException(socket.GetError());
            }
        }

        public void Disconnect() {
            socket.Close();
            socket = null;
        }

        void OnComplete() {
            if (!socket.IsConnected) {
                socket = null;
            }

            completeCallback();
        }
    }
}