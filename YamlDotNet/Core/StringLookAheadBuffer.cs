using System;

namespace YamlDotNet.Core {
    [Serializable]
    class StringLookAheadBuffer : ILookAheadBuffer {
        readonly string value;

        public StringLookAheadBuffer(string value) => this.value = value;

        public int Position { get; private set; }

        public int Length => value.Length;

        public bool EndOfInput => IsOutside(Position);

        public char Peek(int offset) {
            int index = Position + offset;
            return !IsOutside(index) ? value[index] : '\0';
        }

        public void Skip(int length) {
            if (length < 0) {
                throw new ArgumentOutOfRangeException("length", "The length must be positive.");
            }

            Position += length;
        }

        bool IsOutside(int index) => index >= value.Length;
    }
}