using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public abstract class Token {
        protected Token(Mark start, Mark end) {
            Start = start;
            End = end;
        }

        public Mark Start { get; }

        public Mark End { get; }
    }
}