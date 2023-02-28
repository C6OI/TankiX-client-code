using System;

namespace Steamworks {
    [Serializable]
    public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe> {
        public int m_HSteamPipe;

        public HSteamPipe(int value) => m_HSteamPipe = value;

        public int CompareTo(HSteamPipe other) => m_HSteamPipe.CompareTo(other.m_HSteamPipe);

        public bool Equals(HSteamPipe other) => m_HSteamPipe == other.m_HSteamPipe;

        public override string ToString() => m_HSteamPipe.ToString();

        public override bool Equals(object other) => other is HSteamPipe && this == (HSteamPipe)other;

        public override int GetHashCode() => m_HSteamPipe.GetHashCode();

        public static bool operator ==(HSteamPipe x, HSteamPipe y) => x.m_HSteamPipe == y.m_HSteamPipe;

        public static bool operator !=(HSteamPipe x, HSteamPipe y) => !(x == y);

        public static explicit operator HSteamPipe(int value) => new(value);

        public static explicit operator int(HSteamPipe that) => that.m_HSteamPipe;
    }
}