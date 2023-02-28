using System;

namespace Steamworks {
    [Serializable]
    public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t> {
        public int m_SteamItemDef;

        public SteamItemDef_t(int value) => m_SteamItemDef = value;

        public int CompareTo(SteamItemDef_t other) => m_SteamItemDef.CompareTo(other.m_SteamItemDef);

        public bool Equals(SteamItemDef_t other) => m_SteamItemDef == other.m_SteamItemDef;

        public override string ToString() => m_SteamItemDef.ToString();

        public override bool Equals(object other) => other is SteamItemDef_t && this == (SteamItemDef_t)other;

        public override int GetHashCode() => m_SteamItemDef.GetHashCode();

        public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y) => x.m_SteamItemDef == y.m_SteamItemDef;

        public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y) => !(x == y);

        public static explicit operator SteamItemDef_t(int value) => new(value);

        public static explicit operator int(SteamItemDef_t that) => that.m_SteamItemDef;
    }
}