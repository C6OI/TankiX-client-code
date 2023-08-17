using System;
using System.Runtime.Serialization;

namespace YamlDotNet.Core {
    [Serializable]
    public class DuplicateAnchorException : YamlException {
        public DuplicateAnchorException() { }

        public DuplicateAnchorException(string message)
            : base(message) { }

        public DuplicateAnchorException(Mark start, Mark end, string message)
            : base(start, end, message) { }

        public DuplicateAnchorException(string message, Exception inner)
            : base(message, inner) { }

        protected DuplicateAnchorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}