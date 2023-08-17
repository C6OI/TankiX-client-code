using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public struct IndexRange {
        public int Position { get; private set; }

        public int Count { get; private set; }

        [ProtocolTransient] public int EndPosition => Position + Count - 1;

        [ProtocolTransient] public bool Empty => Count == 0;

        public static IndexRange CreateFromPositionAndCount(int position, int count) {
            IndexRange result = default;
            result.Position = Mathf.Max(0, position);
            result.Count = Mathf.Max(0, count);
            return result;
        }

        public static IndexRange CreateFromBeginAndEnd(int position, int endPosition) {
            IndexRange result = default;
            result.Position = Mathf.Max(0, position);
            result.Count = Mathf.Max(0, endPosition - result.Position + 1);
            return result;
        }

        public void CalculateDifference(IndexRange newRange, out IndexRange removedLow, out IndexRange removedHigh,
            out IndexRange addedLow, out IndexRange addedHigh) {
            removedLow = default;
            removedHigh = default;
            addedLow = default;
            addedHigh = default;

            if (newRange.Position > Position) {
                removedLow.Position = Position;
                removedLow.Count = Mathf.Min(newRange.Position, EndPosition + 1) - Position;
            } else if (newRange.Position < Position) {
                addedLow.Position = newRange.Position;
                addedLow.Count = Mathf.Min(Position, newRange.EndPosition + 1) - newRange.Position;
            }

            if (newRange.EndPosition < EndPosition) {
                removedHigh.Position = Mathf.Max(newRange.EndPosition + 1, Position);
                removedHigh.Count = EndPosition - removedHigh.Position + 1;
            } else if (newRange.EndPosition > EndPosition) {
                addedHigh.Position = Mathf.Max(EndPosition + 1, newRange.Position);
                addedHigh.Count = newRange.EndPosition - addedHigh.Position + 1;
            }
        }

        public bool Contains(int index) => index >= Position && index <= EndPosition;

        public IndexRange Intersect(IndexRange range) {
            int position = Mathf.Max(Position, range.Position);
            int endPosition = Mathf.Min(EndPosition, range.EndPosition);
            return CreateFromBeginAndEnd(position, endPosition);
        }

        public void Reset() {
            Position = 0;
            Count = 0;
        }

        public override string ToString() => string.Format("[{0}-{1}]", Position, EndPosition);

        public static bool operator ==(IndexRange a, IndexRange b) => a.Position == b.Position && a.Count == b.Count;

        public static bool operator !=(IndexRange a, IndexRange b) => !(a == b);
    }
}