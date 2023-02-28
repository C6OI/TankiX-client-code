using System;

namespace Steamworks {
    [Serializable]
    public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle> {
        public static readonly ScreenshotHandle Invalid = new(0u);

        public uint m_ScreenshotHandle;

        public ScreenshotHandle(uint value) => m_ScreenshotHandle = value;

        public int CompareTo(ScreenshotHandle other) => m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);

        public bool Equals(ScreenshotHandle other) => m_ScreenshotHandle == other.m_ScreenshotHandle;

        public override string ToString() => m_ScreenshotHandle.ToString();

        public override bool Equals(object other) => other is ScreenshotHandle && this == (ScreenshotHandle)other;

        public override int GetHashCode() => m_ScreenshotHandle.GetHashCode();

        public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y) => x.m_ScreenshotHandle == y.m_ScreenshotHandle;

        public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y) => !(x == y);

        public static explicit operator ScreenshotHandle(uint value) => new(value);

        public static explicit operator uint(ScreenshotHandle that) => that.m_ScreenshotHandle;
    }
}