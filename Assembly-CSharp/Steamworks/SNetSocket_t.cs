using System;

namespace Steamworks {
    [Serializable]
    public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t> {
        public uint m_SNetSocket;

        public SNetSocket_t(uint value) => m_SNetSocket = value;

        public int CompareTo(SNetSocket_t other) => m_SNetSocket.CompareTo(other.m_SNetSocket);

        public bool Equals(SNetSocket_t other) => m_SNetSocket == other.m_SNetSocket;

        public override string ToString() => m_SNetSocket.ToString();

        public override bool Equals(object other) => other is SNetSocket_t && this == (SNetSocket_t)other;

        public override int GetHashCode() => m_SNetSocket.GetHashCode();

        public static bool operator ==(SNetSocket_t x, SNetSocket_t y) => x.m_SNetSocket == y.m_SNetSocket;

        public static bool operator !=(SNetSocket_t x, SNetSocket_t y) => !(x == y);

        public static explicit operator SNetSocket_t(uint value) => new(value);

        public static explicit operator uint(SNetSocket_t that) => that.m_SNetSocket;
    }
}