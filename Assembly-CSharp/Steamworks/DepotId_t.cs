using System;

namespace Steamworks {
    [Serializable]
    public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t> {
        public static readonly DepotId_t Invalid = new(0u);

        public uint m_DepotId;

        public DepotId_t(uint value) => m_DepotId = value;

        public int CompareTo(DepotId_t other) => m_DepotId.CompareTo(other.m_DepotId);

        public bool Equals(DepotId_t other) => m_DepotId == other.m_DepotId;

        public override string ToString() => m_DepotId.ToString();

        public override bool Equals(object other) => other is DepotId_t && this == (DepotId_t)other;

        public override int GetHashCode() => m_DepotId.GetHashCode();

        public static bool operator ==(DepotId_t x, DepotId_t y) => x.m_DepotId == y.m_DepotId;

        public static bool operator !=(DepotId_t x, DepotId_t y) => !(x == y);

        public static explicit operator DepotId_t(uint value) => new(value);

        public static explicit operator uint(DepotId_t that) => that.m_DepotId;
    }
}