using System;

namespace SharpCompress.Compressor.Deflate {
    public class ZlibException : Exception {
        public ZlibException() { }

        public ZlibException(string s)
            : base(s) { }
    }
}