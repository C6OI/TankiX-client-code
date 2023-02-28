namespace SharpCompress.Compressor.Deflate {
    internal class WorkItem {
        public byte[] buffer;

        public byte[] compressed;

        public int compressedBytesAvailable;

        public ZlibCodec compressor;

        public int crc;

        public int index;

        public int inputBytesAvailable;

        public int status;

        public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy) {
            buffer = new byte[size];
            int num = size + (size / 32768 + 1) * 5 * 2;
            compressed = new byte[num];
            status = 0;
            compressor = new ZlibCodec();
            compressor.InitializeDeflate(compressLevel, false);
            compressor.OutputBuffer = compressed;
            compressor.InputBuffer = buffer;
        }

        internal enum Status {
            None = 0,
            Filling = 1,
            Filled = 2,
            Compressing = 3,
            Compressed = 4,
            Writing = 5,
            Done = 6
        }
    }
}