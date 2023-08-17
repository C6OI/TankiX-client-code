using System;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public static class EnumerableExtension {
        public static void ForEach<TKey>(this IEnumerable<TKey> enumerable, Action<TKey> compute) {
            IEnumerator<TKey> enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext()) {
                compute(enumerator.Current);
            }
        }

        public static void ForEach<TKey>(this IList<TKey> list, Action<TKey> compute) {
            int count = list.Count;

            for (int i = 0; i < count; i++) {
                compute(list[i]);
            }
        }
    }
}