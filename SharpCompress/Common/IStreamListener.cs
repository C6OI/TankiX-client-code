namespace SharpCompress.Common {
    interface IStreamListener {
        void FireFilePartExtractionBegin(string name, long size, long compressedSize);

        void FireCompressedBytesRead(long currentPartCompressedBytes, long compressedReadBytes);
    }
}