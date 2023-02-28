using System;

namespace SharpCompress.Compressor.LZMA {
    internal class DataErrorException : Exception {
        public DataErrorException()
            : base("Data Error") { }
    }
}