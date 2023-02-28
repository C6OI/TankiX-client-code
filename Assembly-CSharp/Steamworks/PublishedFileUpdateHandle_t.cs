using System;

namespace Steamworks {
    [Serializable]
    public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t> {
        public static readonly PublishedFileUpdateHandle_t Invalid = new(ulong.MaxValue);

        public ulong m_PublishedFileUpdateHandle;

        public PublishedFileUpdateHandle_t(ulong value) => m_PublishedFileUpdateHandle = value;

        public int CompareTo(PublishedFileUpdateHandle_t other) => m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);

        public bool Equals(PublishedFileUpdateHandle_t other) => m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;

        public override string ToString() => m_PublishedFileUpdateHandle.ToString();

        public override bool Equals(object other) => other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t)other;

        public override int GetHashCode() => m_PublishedFileUpdateHandle.GetHashCode();

        public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;

        public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => !(x == y);

        public static explicit operator PublishedFileUpdateHandle_t(ulong value) => new(value);

        public static explicit operator ulong(PublishedFileUpdateHandle_t that) => that.m_PublishedFileUpdateHandle;
    }
}