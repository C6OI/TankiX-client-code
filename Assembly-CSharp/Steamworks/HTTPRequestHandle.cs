using System;

namespace Steamworks {
    [Serializable]
    public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle> {
        public static readonly HTTPRequestHandle Invalid = new(0u);

        public uint m_HTTPRequestHandle;

        public HTTPRequestHandle(uint value) => m_HTTPRequestHandle = value;

        public int CompareTo(HTTPRequestHandle other) => m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);

        public bool Equals(HTTPRequestHandle other) => m_HTTPRequestHandle == other.m_HTTPRequestHandle;

        public override string ToString() => m_HTTPRequestHandle.ToString();

        public override bool Equals(object other) => other is HTTPRequestHandle && this == (HTTPRequestHandle)other;

        public override int GetHashCode() => m_HTTPRequestHandle.GetHashCode();

        public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y) => x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;

        public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y) => !(x == y);

        public static explicit operator HTTPRequestHandle(uint value) => new(value);

        public static explicit operator uint(HTTPRequestHandle that) => that.m_HTTPRequestHandle;
    }
}