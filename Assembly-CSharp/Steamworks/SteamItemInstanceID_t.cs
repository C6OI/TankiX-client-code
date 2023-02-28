using System;

namespace Steamworks {
    [Serializable]
    public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t> {
        public static readonly SteamItemInstanceID_t Invalid = new(ulong.MaxValue);

        public ulong m_SteamItemInstanceID;

        public SteamItemInstanceID_t(ulong value) => m_SteamItemInstanceID = value;

        public int CompareTo(SteamItemInstanceID_t other) => m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);

        public bool Equals(SteamItemInstanceID_t other) => m_SteamItemInstanceID == other.m_SteamItemInstanceID;

        public override string ToString() => m_SteamItemInstanceID.ToString();

        public override bool Equals(object other) => other is SteamItemInstanceID_t && this == (SteamItemInstanceID_t)other;

        public override int GetHashCode() => m_SteamItemInstanceID.GetHashCode();

        public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;

        public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => !(x == y);

        public static explicit operator SteamItemInstanceID_t(ulong value) => new(value);

        public static explicit operator ulong(SteamItemInstanceID_t that) => that.m_SteamItemInstanceID;
    }
}