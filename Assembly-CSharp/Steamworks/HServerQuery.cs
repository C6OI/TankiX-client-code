using System;

namespace Steamworks {
    [Serializable]
    public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery> {
        public static readonly HServerQuery Invalid = new(-1);

        public int m_HServerQuery;

        public HServerQuery(int value) => m_HServerQuery = value;

        public int CompareTo(HServerQuery other) => m_HServerQuery.CompareTo(other.m_HServerQuery);

        public bool Equals(HServerQuery other) => m_HServerQuery == other.m_HServerQuery;

        public override string ToString() => m_HServerQuery.ToString();

        public override bool Equals(object other) => other is HServerQuery && this == (HServerQuery)other;

        public override int GetHashCode() => m_HServerQuery.GetHashCode();

        public static bool operator ==(HServerQuery x, HServerQuery y) => x.m_HServerQuery == y.m_HServerQuery;

        public static bool operator !=(HServerQuery x, HServerQuery y) => !(x == y);

        public static explicit operator HServerQuery(int value) => new(value);

        public static explicit operator int(HServerQuery that) => that.m_HServerQuery;
    }
}