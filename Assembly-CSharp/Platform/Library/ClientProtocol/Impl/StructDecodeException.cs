using System;

namespace Platform.Library.ClientProtocol.Impl {
    public class StructDecodeException : Exception {
        public StructDecodeException(object structInstance, Exception e)
            : base(string.Format("partial struct={0}", structInstance), e) { }
    }
}