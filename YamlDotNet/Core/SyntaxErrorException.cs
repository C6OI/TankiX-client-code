using System;
using System.Runtime.Serialization;

namespace YamlDotNet.Core {
    [Serializable]
    public class SyntaxErrorException : YamlException {
        public SyntaxErrorException() { }

        public SyntaxErrorException(string message)
            : base(message) { }

        public SyntaxErrorException(Mark start, Mark end, string message)
            : base(start, end, message) { }

        public SyntaxErrorException(string message, Exception inner)
            : base(message, inner) { }

        protected SyntaxErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}