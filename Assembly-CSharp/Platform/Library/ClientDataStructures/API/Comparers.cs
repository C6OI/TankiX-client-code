using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Library.ClientDataStructures.Impl;

namespace Platform.Library.ClientDataStructures.API {
    public class Comparers {
        public static IComparer GetComparer(Type type) {
            if (typeof(IComparable).IsAssignableFrom(type)) {
                return Comparer.Default;
            }

            throw new TypeIsNotComparableException(type);
        }

        public static IComparer<T> GetComparer<T>() {
            if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)) || typeof(IComparable).IsAssignableFrom(typeof(T))) {
                return Comparer<T>.Default;
            }

            throw new TypeIsNotComparableException(typeof(T));
        }

        public static IComparer<T> GetComparer<T>(Comparison<T> comparison) => new ComparisonComparer<T>(comparison);

        class ComparisonComparer<T> : IComparer<T> {
            readonly Comparison<T> comparison;

            public ComparisonComparer(Comparison<T> comparison) => this.comparison = comparison;

            public int Compare(T x, T y) => comparison(x, y);
        }
    }
}