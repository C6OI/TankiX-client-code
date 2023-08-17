using System;

namespace YamlDotNet.Core {
    [Serializable]
    class Cursor {
        public Cursor() => Line = 1;

        public Cursor(Cursor cursor) {
            Index = cursor.Index;
            Line = cursor.Line;
            LineOffset = cursor.LineOffset;
        }

        public int Index { get; set; }

        public int Line { get; set; }

        public int LineOffset { get; set; }

        public Mark Mark() => new(Index, Line, LineOffset + 1);

        public void Skip() {
            Index++;
            LineOffset++;
        }

        public void SkipLineByOffset(int offset) {
            Index += offset;
            Line++;
            LineOffset = 0;
        }

        public void ForceSkipLineAfterNonBreak() {
            if (LineOffset != 0) {
                Line++;
                LineOffset = 0;
            }
        }
    }
}