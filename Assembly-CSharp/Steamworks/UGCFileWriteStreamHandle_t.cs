using System;

namespace Steamworks {
    [Serializable]
    public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t> {
        public static readonly UGCFileWriteStreamHandle_t Invalid = new(ulong.MaxValue);

        public ulong m_UGCFileWriteStreamHandle;

        public UGCFileWriteStreamHandle_t(ulong value) => m_UGCFileWriteStreamHandle = value;

        public int CompareTo(UGCFileWriteStreamHandle_t other) => m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);

        public bool Equals(UGCFileWriteStreamHandle_t other) => m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;

        public override string ToString() => m_UGCFileWriteStreamHandle.ToString();

        public override bool Equals(object other) => other is UGCFileWriteStreamHandle_t && this == (UGCFileWriteStreamHandle_t)other;

        public override int GetHashCode() => m_UGCFileWriteStreamHandle.GetHashCode();

        public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y) => x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;

        public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y) => !(x == y);

        public static explicit operator UGCFileWriteStreamHandle_t(ulong value) => new(value);

        public static explicit operator ulong(UGCFileWriteStreamHandle_t that) => that.m_UGCFileWriteStreamHandle;
    }
}