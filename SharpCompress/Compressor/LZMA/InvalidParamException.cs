using System;

namespace SharpCompress.Compressor.LZMA {
    class InvalidParamException : Exception {
        public InvalidParamException()
            : base("Invalid Parameter") { }
    }
}