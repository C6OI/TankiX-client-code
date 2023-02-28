using System;

namespace Steamworks {
    [Serializable]
    public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t> {
        public static readonly PublishedFileId_t Invalid = new(0uL);

        public ulong m_PublishedFileId;

        public PublishedFileId_t(ulong value) => m_PublishedFileId = value;

        public int CompareTo(PublishedFileId_t other) => m_PublishedFileId.CompareTo(other.m_PublishedFileId);

        public bool Equals(PublishedFileId_t other) => m_PublishedFileId == other.m_PublishedFileId;

        public override string ToString() => m_PublishedFileId.ToString();

        public override bool Equals(object other) => other is PublishedFileId_t && this == (PublishedFileId_t)other;

        public override int GetHashCode() => m_PublishedFileId.GetHashCode();

        public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y) => x.m_PublishedFileId == y.m_PublishedFileId;

        public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y) => !(x == y);

        public static explicit operator PublishedFileId_t(ulong value) => new(value);

        public static explicit operator ulong(PublishedFileId_t that) => that.m_PublishedFileId;
    }
}