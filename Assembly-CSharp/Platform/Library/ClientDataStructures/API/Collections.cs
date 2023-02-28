using System;
using System.Collections.Generic;
using Platform.Library.ClientDataStructures.Impl;

namespace Platform.Library.ClientDataStructures.API {
    public class Collections {
        public static readonly object[] EmptyArray = new object[0];

        public static IList<T> EmptyList<T>() => Impl.EmptyList<T>.Instance;

        public static List<T> AsList<T>(params T[] values) => new(values);

        public static IList<T> SingletonList<T>(T value) => new SingletonList<T>(value);

        public static T GetOnlyElement<T>(ICollection<T> coll) {
            if (coll.Count != 1) {
                throw new InvalidOperationException("Count: " + coll.Count);
            }

            List<T> list;

            if ((list = coll as List<T>) != null) {
                return list[0];
            }

            HashSet<T> hashSet;

            if ((hashSet = coll as HashSet<T>) != null) {
                HashSet<T>.Enumerator enumerator = hashSet.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current;
            }

            IEnumerator<T> enumerator2 = coll.GetEnumerator();
            enumerator2.MoveNext();
            return enumerator2.Current;
        }

        public static Enumerator<T> GetEnumerator<T>(IEnumerable<T> collection) => new(collection);

        public static void ForEach<T>(IEnumerable<T> coll, Action<T> action) {
            Enumerator<T> enumerator = GetEnumerator(coll);

            while (enumerator.MoveNext()) {
                action(enumerator.Current);
            }
        }

        public struct Enumerator<T> {
            readonly IEnumerable<T> collection;

            HashSet<T>.Enumerator hashSetEnumerator;

            List<T>.Enumerator ListEnumerator;

            readonly IEnumerator<T> enumerator;

            public T Current {
                get {
                    if (collection is List<T>) {
                        return ListEnumerator.Current;
                    }

                    if (collection is HashSet<T>) {
                        return hashSetEnumerator.Current;
                    }

                    return enumerator.Current;
                }
            }

            public Enumerator(IEnumerable<T> collection) {
                this.collection = collection;
                enumerator = null;
                List<T> list;
                HashSet<T> hashSet;

                if ((list = collection as List<T>) != null) {
                    ListEnumerator = list.GetEnumerator();
                    hashSetEnumerator = default;
                } else if ((hashSet = collection as HashSet<T>) != null) {
                    hashSetEnumerator = hashSet.GetEnumerator();
                    ListEnumerator = default;
                } else {
                    hashSetEnumerator = default;
                    ListEnumerator = default;
                    enumerator = collection.GetEnumerator();
                }
            }

            public bool MoveNext() {
                if (collection is List<T>) {
                    return ListEnumerator.MoveNext();
                }

                if (collection is HashSet<T>) {
                    return hashSetEnumerator.MoveNext();
                }

                return enumerator.MoveNext();
            }
        }
    }
}