using System;

namespace Steamworks {
    [Serializable]
    public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser> {
        public int m_HSteamUser;

        public HSteamUser(int value) => m_HSteamUser = value;

        public int CompareTo(HSteamUser other) => m_HSteamUser.CompareTo(other.m_HSteamUser);

        public bool Equals(HSteamUser other) => m_HSteamUser == other.m_HSteamUser;

        public override string ToString() => m_HSteamUser.ToString();

        public override bool Equals(object other) => other is HSteamUser && this == (HSteamUser)other;

        public override int GetHashCode() => m_HSteamUser.GetHashCode();

        public static bool operator ==(HSteamUser x, HSteamUser y) => x.m_HSteamUser == y.m_HSteamUser;

        public static bool operator !=(HSteamUser x, HSteamUser y) => !(x == y);

        public static explicit operator HSteamUser(int value) => new(value);

        public static explicit operator int(HSteamUser that) => that.m_HSteamUser;
    }
}