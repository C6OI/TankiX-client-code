using System;

namespace YamlDotNet.Core {
    [Serializable]
    public class Mark : IComparable, IEquatable<Mark>, IComparable<Mark> {
        public static readonly Mark Empty = new();

        public Mark() {
            Line = 1;
            Column = 1;
        }

        public Mark(int index, int line, int column) {
            if (index < 0) {
                throw new ArgumentOutOfRangeException("index", "Index must be greater than or equal to zero.");
            }

            if (line < 1) {
                throw new ArgumentOutOfRangeException("line", "Line must be greater than or equal to 1.");
            }

            if (column < 1) {
                throw new ArgumentOutOfRangeException("column", "Column must be greater than or equal to 1.");
            }

            Index = index;
            Line = line;
            Column = column;
        }

        public int Index { get; }

        public int Line { get; }

        public int Column { get; }

        public int CompareTo(object obj) {
            if (obj == null) {
                throw new ArgumentNullException("obj");
            }

            return CompareTo(obj as Mark);
        }

        public int CompareTo(Mark other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }

            int num = Line.CompareTo(other.Line);

            if (num == 0) {
                num = Column.CompareTo(other.Column);
            }

            return num;
        }

        public bool Equals(Mark other) =>
            other != null && Index == other.Index && Line == other.Line && Column == other.Column;

        public override string ToString() => string.Format("Line: {0}, Col: {1}, Idx: {2}", Line, Column, Index);

        public override bool Equals(object obj) => Equals(obj as Mark);

        public override int GetHashCode() => HashCode.CombineHashCodes(Index.GetHashCode(),
            HashCode.CombineHashCodes(Line.GetHashCode(), Column.GetHashCode()));
    }
}