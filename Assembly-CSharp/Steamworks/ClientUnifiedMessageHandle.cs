using System;

namespace Steamworks {
    [Serializable]
    public struct ClientUnifiedMessageHandle : IEquatable<ClientUnifiedMessageHandle>, IComparable<ClientUnifiedMessageHandle> {
        public static readonly ClientUnifiedMessageHandle Invalid = new(0uL);

        public ulong m_ClientUnifiedMessageHandle;

        public ClientUnifiedMessageHandle(ulong value) => m_ClientUnifiedMessageHandle = value;

        public int CompareTo(ClientUnifiedMessageHandle other) => m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);

        public bool Equals(ClientUnifiedMessageHandle other) => m_ClientUnifiedMessageHandle == other.m_ClientUnifiedMessageHandle;

        public override string ToString() => m_ClientUnifiedMessageHandle.ToString();

        public override bool Equals(object other) => other is ClientUnifiedMessageHandle && this == (ClientUnifiedMessageHandle)other;

        public override int GetHashCode() => m_ClientUnifiedMessageHandle.GetHashCode();

        public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => x.m_ClientUnifiedMessageHandle == y.m_ClientUnifiedMessageHandle;

        public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => !(x == y);

        public static explicit operator ClientUnifiedMessageHandle(ulong value) => new(value);

        public static explicit operator ulong(ClientUnifiedMessageHandle that) => that.m_ClientUnifiedMessageHandle;
    }
}