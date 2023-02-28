using System;

namespace Steamworks {
    [Serializable]
    public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t> {
        public static readonly SteamAPICall_t Invalid = new(0uL);

        public ulong m_SteamAPICall;

        public SteamAPICall_t(ulong value) => m_SteamAPICall = value;

        public int CompareTo(SteamAPICall_t other) => m_SteamAPICall.CompareTo(other.m_SteamAPICall);

        public bool Equals(SteamAPICall_t other) => m_SteamAPICall == other.m_SteamAPICall;

        public override string ToString() => m_SteamAPICall.ToString();

        public override bool Equals(object other) => other is SteamAPICall_t && this == (SteamAPICall_t)other;

        public override int GetHashCode() => m_SteamAPICall.GetHashCode();

        public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y) => x.m_SteamAPICall == y.m_SteamAPICall;

        public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y) => !(x == y);

        public static explicit operator SteamAPICall_t(ulong value) => new(value);

        public static explicit operator ulong(SteamAPICall_t that) => that.m_SteamAPICall;
    }
}