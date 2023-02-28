using System;

namespace Steamworks {
    [Serializable]
    public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t> {
        public static readonly SteamInventoryResult_t Invalid = new(-1);

        public int m_SteamInventoryResult;

        public SteamInventoryResult_t(int value) => m_SteamInventoryResult = value;

        public int CompareTo(SteamInventoryResult_t other) => m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);

        public bool Equals(SteamInventoryResult_t other) => m_SteamInventoryResult == other.m_SteamInventoryResult;

        public override string ToString() => m_SteamInventoryResult.ToString();

        public override bool Equals(object other) => other is SteamInventoryResult_t && this == (SteamInventoryResult_t)other;

        public override int GetHashCode() => m_SteamInventoryResult.GetHashCode();

        public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y) => x.m_SteamInventoryResult == y.m_SteamInventoryResult;

        public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y) => !(x == y);

        public static explicit operator SteamInventoryResult_t(int value) => new(value);

        public static explicit operator int(SteamInventoryResult_t that) => that.m_SteamInventoryResult;
    }
}