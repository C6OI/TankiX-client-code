using System;

namespace Steamworks {
    [Serializable]
    public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t> {
        public static readonly UGCQueryHandle_t Invalid = new(ulong.MaxValue);

        public ulong m_UGCQueryHandle;

        public UGCQueryHandle_t(ulong value) => m_UGCQueryHandle = value;

        public int CompareTo(UGCQueryHandle_t other) => m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);

        public bool Equals(UGCQueryHandle_t other) => m_UGCQueryHandle == other.m_UGCQueryHandle;

        public override string ToString() => m_UGCQueryHandle.ToString();

        public override bool Equals(object other) => other is UGCQueryHandle_t && this == (UGCQueryHandle_t)other;

        public override int GetHashCode() => m_UGCQueryHandle.GetHashCode();

        public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y) => x.m_UGCQueryHandle == y.m_UGCQueryHandle;

        public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y) => !(x == y);

        public static explicit operator UGCQueryHandle_t(ulong value) => new(value);

        public static explicit operator ulong(UGCQueryHandle_t that) => that.m_UGCQueryHandle;
    }
}