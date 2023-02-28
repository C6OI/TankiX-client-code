using System;

namespace Steamworks {
    [Serializable]
    public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t> {
        public ulong m_SteamLeaderboard;

        public SteamLeaderboard_t(ulong value) => m_SteamLeaderboard = value;

        public int CompareTo(SteamLeaderboard_t other) => m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);

        public bool Equals(SteamLeaderboard_t other) => m_SteamLeaderboard == other.m_SteamLeaderboard;

        public override string ToString() => m_SteamLeaderboard.ToString();

        public override bool Equals(object other) => other is SteamLeaderboard_t && this == (SteamLeaderboard_t)other;

        public override int GetHashCode() => m_SteamLeaderboard.GetHashCode();

        public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y) => x.m_SteamLeaderboard == y.m_SteamLeaderboard;

        public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y) => !(x == y);

        public static explicit operator SteamLeaderboard_t(ulong value) => new(value);

        public static explicit operator ulong(SteamLeaderboard_t that) => that.m_SteamLeaderboard;
    }
}