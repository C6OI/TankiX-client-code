using System;

namespace Steamworks {
    [Serializable]
    public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser> {
        public static readonly HHTMLBrowser Invalid = new(0u);

        public uint m_HHTMLBrowser;

        public HHTMLBrowser(uint value) => m_HHTMLBrowser = value;

        public int CompareTo(HHTMLBrowser other) => m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);

        public bool Equals(HHTMLBrowser other) => m_HHTMLBrowser == other.m_HHTMLBrowser;

        public override string ToString() => m_HHTMLBrowser.ToString();

        public override bool Equals(object other) => other is HHTMLBrowser && this == (HHTMLBrowser)other;

        public override int GetHashCode() => m_HHTMLBrowser.GetHashCode();

        public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y) => x.m_HHTMLBrowser == y.m_HHTMLBrowser;

        public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y) => !(x == y);

        public static explicit operator HHTMLBrowser(uint value) => new(value);

        public static explicit operator uint(HHTMLBrowser that) => that.m_HHTMLBrowser;
    }
}