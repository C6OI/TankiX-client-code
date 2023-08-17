using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class Tag : Token {
        public Tag(string handle, string suffix)
            : this(handle, suffix, Mark.Empty, Mark.Empty) { }

        public Tag(string handle, string suffix, Mark start, Mark end)
            : base(start, end) {
            Handle = handle;
            Suffix = suffix;
        }

        public string Handle { get; }

        public string Suffix { get; }
    }
}