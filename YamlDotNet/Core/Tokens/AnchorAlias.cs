using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class AnchorAlias : Token {
        public AnchorAlias(string value)
            : this(value, Mark.Empty, Mark.Empty) { }

        public AnchorAlias(string value, Mark start, Mark end)
            : base(start, end) => Value = value;

        public string Value { get; }
    }
}