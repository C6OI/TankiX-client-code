using System;

namespace Steamworks {
    [Serializable]
    public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t> {
        public static readonly ManifestId_t Invalid = new(0uL);

        public ulong m_ManifestId;

        public ManifestId_t(ulong value) => m_ManifestId = value;

        public int CompareTo(ManifestId_t other) => m_ManifestId.CompareTo(other.m_ManifestId);

        public bool Equals(ManifestId_t other) => m_ManifestId == other.m_ManifestId;

        public override string ToString() => m_ManifestId.ToString();

        public override bool Equals(object other) => other is ManifestId_t && this == (ManifestId_t)other;

        public override int GetHashCode() => m_ManifestId.GetHashCode();

        public static bool operator ==(ManifestId_t x, ManifestId_t y) => x.m_ManifestId == y.m_ManifestId;

        public static bool operator !=(ManifestId_t x, ManifestId_t y) => !(x == y);

        public static explicit operator ManifestId_t(ulong value) => new(value);

        public static explicit operator ulong(ManifestId_t that) => that.m_ManifestId;
    }
}