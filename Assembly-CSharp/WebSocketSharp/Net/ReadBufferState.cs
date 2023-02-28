namespace WebSocketSharp.Net {
    internal class ReadBufferState {
        public ReadBufferState(byte[] buffer, int offset, int count, HttpStreamAsyncResult asyncResult) {
            Buffer = buffer;
            Offset = offset;
            Count = count;
            InitialCount = count;
            AsyncResult = asyncResult;
        }

        public HttpStreamAsyncResult AsyncResult { get; set; }

        public byte[] Buffer { get; set; }

        public int Count { get; set; }

        public int InitialCount { get; set; }

        public int Offset { get; set; }
    }
}