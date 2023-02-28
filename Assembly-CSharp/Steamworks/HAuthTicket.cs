using System;

namespace Steamworks {
    [Serializable]
    public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket> {
        public static readonly HAuthTicket Invalid = new(0u);

        public uint m_HAuthTicket;

        public HAuthTicket(uint value) => m_HAuthTicket = value;

        public int CompareTo(HAuthTicket other) => m_HAuthTicket.CompareTo(other.m_HAuthTicket);

        public bool Equals(HAuthTicket other) => m_HAuthTicket == other.m_HAuthTicket;

        public override string ToString() => m_HAuthTicket.ToString();

        public override bool Equals(object other) => other is HAuthTicket && this == (HAuthTicket)other;

        public override int GetHashCode() => m_HAuthTicket.GetHashCode();

        public static bool operator ==(HAuthTicket x, HAuthTicket y) => x.m_HAuthTicket == y.m_HAuthTicket;

        public static bool operator !=(HAuthTicket x, HAuthTicket y) => !(x == y);

        public static explicit operator HAuthTicket(uint value) => new(value);

        public static explicit operator uint(HAuthTicket that) => that.m_HAuthTicket;
    }
}