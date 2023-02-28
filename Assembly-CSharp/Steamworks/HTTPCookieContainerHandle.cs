using System;

namespace Steamworks {
    [Serializable]
    public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle> {
        public static readonly HTTPCookieContainerHandle Invalid = new(0u);

        public uint m_HTTPCookieContainerHandle;

        public HTTPCookieContainerHandle(uint value) => m_HTTPCookieContainerHandle = value;

        public int CompareTo(HTTPCookieContainerHandle other) => m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);

        public bool Equals(HTTPCookieContainerHandle other) => m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;

        public override string ToString() => m_HTTPCookieContainerHandle.ToString();

        public override bool Equals(object other) => other is HTTPCookieContainerHandle && this == (HTTPCookieContainerHandle)other;

        public override int GetHashCode() => m_HTTPCookieContainerHandle.GetHashCode();

        public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;

        public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => !(x == y);

        public static explicit operator HTTPCookieContainerHandle(uint value) => new(value);

        public static explicit operator uint(HTTPCookieContainerHandle that) => that.m_HTTPCookieContainerHandle;
    }
}