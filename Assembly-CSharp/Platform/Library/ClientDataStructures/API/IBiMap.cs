using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public interface IBiMap<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable {
        IBiMap<TValue, TKey> Inverse { get; }

        bool Contains(TKey key, TValue value);

        void ForcePut(TKey key, TValue value);

        bool Remove(TKey key, TValue value);
    }
}