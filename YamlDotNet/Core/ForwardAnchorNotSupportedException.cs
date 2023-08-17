using System;
using System.Runtime.Serialization;

namespace YamlDotNet.Core {
    [Serializable]
    public class ForwardAnchorNotSupportedException : YamlException {
        public ForwardAnchorNotSupportedException() { }

        public ForwardAnchorNotSupportedException(string message)
            : base(message) { }

        public ForwardAnchorNotSupportedException(Mark start, Mark end, string message)
            : base(start, end, message) { }

        public ForwardAnchorNotSupportedException(string message, Exception inner)
            : base(message, inner) { }

        protected ForwardAnchorNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}