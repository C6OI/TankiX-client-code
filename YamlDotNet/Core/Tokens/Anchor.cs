using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class Anchor : Token {
        public Anchor(string value)
            : this(value, Mark.Empty, Mark.Empty) { }

        public Anchor(string value, Mark start, Mark end)
            : base(start, end) => Value = value;

        public string Value { get; }
    }
}