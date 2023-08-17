using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class CodecNotFoundForRequestException : Exception {
        public CodecNotFoundForRequestException(CodecInfo request)
            : base("request = " + request) { }
    }
}