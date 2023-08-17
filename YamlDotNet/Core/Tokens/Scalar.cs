using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class Scalar : Token {
        public Scalar(string value)
            : this(value, ScalarStyle.Any) { }

        public Scalar(string value, ScalarStyle style)
            : this(value, style, Mark.Empty, Mark.Empty) { }

        public Scalar(string value, ScalarStyle style, Mark start, Mark end)
            : base(start, end) {
            Value = value;
            Style = style;
        }

        public string Value { get; }

        public ScalarStyle Style { get; }
    }
}