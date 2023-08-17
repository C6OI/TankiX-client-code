using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class Comment : Token {
        public Comment(string value, bool isInline)
            : this(value, isInline, Mark.Empty, Mark.Empty) { }

        public Comment(string value, bool isInline, Mark start, Mark end)
            : base(start, end) {
            IsInline = isInline;
            Value = value;
        }

        public string Value { get; private set; }

        public bool IsInline { get; private set; }
    }
}