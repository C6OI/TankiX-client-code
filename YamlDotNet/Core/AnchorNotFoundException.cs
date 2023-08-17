using System;
using System.Runtime.Serialization;

namespace YamlDotNet.Core {
    [Serializable]
    public class AnchorNotFoundException : YamlException {
        public AnchorNotFoundException() { }

        public AnchorNotFoundException(string message)
            : base(message) { }

        public AnchorNotFoundException(Mark start, Mark end, string message)
            : base(start, end, message) { }

        public AnchorNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        protected AnchorNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}