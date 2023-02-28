using System;

namespace SharpCompress.Compressor.LZMA {
    internal class InvalidParamException : Exception {
        public InvalidParamException()
            : base("Invalid Parameter") { }
    }
}