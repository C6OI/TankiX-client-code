using System;

namespace Steamworks {
    [Serializable]
    public struct HServerListRequest : IEquatable<HServerListRequest> {
        public static readonly HServerListRequest Invalid = new(IntPtr.Zero);

        public IntPtr m_HServerListRequest;

        public HServerListRequest(IntPtr value) => m_HServerListRequest = value;

        public bool Equals(HServerListRequest other) => m_HServerListRequest == other.m_HServerListRequest;

        public override string ToString() => m_HServerListRequest.ToString();

        public override bool Equals(object other) => other is HServerListRequest && this == (HServerListRequest)other;

        public override int GetHashCode() => m_HServerListRequest.GetHashCode();

        public static bool operator ==(HServerListRequest x, HServerListRequest y) => x.m_HServerListRequest == y.m_HServerListRequest;

        public static bool operator !=(HServerListRequest x, HServerListRequest y) => !(x == y);

        public static explicit operator HServerListRequest(IntPtr value) => new(value);

        public static explicit operator IntPtr(HServerListRequest that) => that.m_HServerListRequest;
    }
}