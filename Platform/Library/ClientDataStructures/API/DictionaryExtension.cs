using System;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public static class DictionaryExtension {
        public static TValue ComputeIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            Func<TKey, TValue> compute) {
            TValue value;

            if (dictionary.TryGetValue(key, out value)) {
                return value;
            }

            TValue val = compute(key);
            dictionary.Add(key, val);
            return val;
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            Func<TValue> compute) {
            TValue value;

            if (dictionary.TryGetValue(key, out value)) {
                return value;
            }

            return compute();
        }
    }
}