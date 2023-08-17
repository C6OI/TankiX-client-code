using System;

namespace SharpCompress.Compressor.LZMA {
    class DataErrorException : Exception {
        public DataErrorException()
            : base("Data Error") { }
    }
}