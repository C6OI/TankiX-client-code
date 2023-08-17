using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public interface IMultiMap<TKey, TValue> : IEnumerable, ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>> {
        ICollection<TValue> this[TKey key] { get; set; }

        ICollection<TKey> Keys { get; }

        ICollection<TValue> Values { get; }

        bool Contains(TKey key, TValue value);

        bool ContainsKey(TKey key);

        bool ContainsValue(TValue value);

        ICollection<KeyValuePair<TKey, TValue>> Entries();

        void Add(TKey key, TValue value);

        void AddAll(TKey key, ICollection<TValue> values);

        bool Remove(TKey key, TValue value);

        ICollection<TValue> RemoveAll(TKey key);
    }
}