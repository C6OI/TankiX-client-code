using System;

namespace Steamworks {
    [Serializable]
    public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t> {
        public uint m_SNetListenSocket;

        public SNetListenSocket_t(uint value) => m_SNetListenSocket = value;

        public int CompareTo(SNetListenSocket_t other) => m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);

        public bool Equals(SNetListenSocket_t other) => m_SNetListenSocket == other.m_SNetListenSocket;

        public override string ToString() => m_SNetListenSocket.ToString();

        public override bool Equals(object other) => other is SNetListenSocket_t && this == (SNetListenSocket_t)other;

        public override int GetHashCode() => m_SNetListenSocket.GetHashCode();

        public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y) => x.m_SNetListenSocket == y.m_SNetListenSocket;

        public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y) => !(x == y);

        public static explicit operator SNetListenSocket_t(uint value) => new(value);

        public static explicit operator uint(SNetListenSocket_t that) => that.m_SNetListenSocket;
    }
}