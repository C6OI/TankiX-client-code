using System;

namespace YamlDotNet.Core {
    [Serializable]
    class CharacterAnalyzer<TBuffer> where TBuffer : ILookAheadBuffer {
        public CharacterAnalyzer(TBuffer buffer) => Buffer = buffer;

        public TBuffer Buffer { get; }

        public bool EndOfInput => Buffer.EndOfInput;

        public char Peek(int offset) => Buffer.Peek(offset);

        public void Skip(int length) => Buffer.Skip(length);

        public bool IsAlphaNumericDashOrUnderscore(int offset = 0) {
            char c = Buffer.Peek(offset);
            return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-';
        }

        public bool IsAscii(int offset = 0) => Buffer.Peek(offset) <= '\u007f';

        public bool IsPrintable(int offset = 0) {
            char c = Buffer.Peek(offset);

            return c == '\t' ||
                   c == '\n' ||
                   c == '\r' ||
                   c >= ' ' && c <= '~' ||
                   c == '\u0085' ||
                   c >= '\u00a0' && c <= '\ud7ff' ||
                   c >= '\ue000' && c <= '\ufffd';
        }

        public bool IsDigit(int offset = 0) {
            char c = Buffer.Peek(offset);
            return c >= '0' && c <= '9';
        }

        public int AsDigit(int offset = 0) => Buffer.Peek(offset) - 48;

        public bool IsHex(int offset) {
            char c = Buffer.Peek(offset);
            return c >= '0' && c <= '9' || c >= 'A' && c <= 'F' || c >= 'a' && c <= 'f';
        }

        public int AsHex(int offset) {
            char c = Buffer.Peek(offset);

            if (c <= '9') {
                return c - 48;
            }

            if (c <= 'F') {
                return c - 65 + 10;
            }

            return c - 97 + 10;
        }

        public bool IsSpace(int offset = 0) => Check(' ', offset);

        public bool IsZero(int offset = 0) => Check('\0', offset);

        public bool IsTab(int offset = 0) => Check('\t', offset);

        public bool IsWhite(int offset = 0) => IsSpace(offset) || IsTab(offset);

        public bool IsBreak(int offset = 0) => Check("\r\n\u0085\u2028\u2029", offset);

        public bool IsCrLf(int offset = 0) => Check('\r', offset) && Check('\n', offset + 1);

        public bool IsBreakOrZero(int offset = 0) => IsBreak(offset) || IsZero(offset);

        public bool IsWhiteBreakOrZero(int offset = 0) => IsWhite(offset) || IsBreakOrZero(offset);

        public bool Check(char expected, int offset = 0) => Buffer.Peek(offset) == expected;

        public bool Check(string expectedCharacters, int offset = 0) {
            char value = Buffer.Peek(offset);
            return expectedCharacters.IndexOf(value) != -1;
        }
    }
}