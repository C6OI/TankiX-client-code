using System;
using System.Collections;
using System.Runtime.Serialization;

namespace log4net.Util {
    [Serializable]
    public sealed class PropertiesDictionary : ReadOnlyPropertiesDictionary, IEnumerable, ICollection, IDictionary,
        ISerializable {
        public PropertiesDictionary() { }

        public PropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
            : base(propertiesDictionary) { }

        PropertiesDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public override object this[string key] {
            get => InnerHashtable[key];
            set => InnerHashtable[key] = value;
        }

        bool ICollection.IsSynchronized => InnerHashtable.IsSynchronized;

        object ICollection.SyncRoot => InnerHashtable.SyncRoot;

        void ICollection.CopyTo(Array array, int index) => InnerHashtable.CopyTo(array, index);

        bool IDictionary.IsReadOnly => false;

        object IDictionary.this[object key] {
            get {
                if (!(key is string)) {
                    throw new ArgumentException("key must be a string", "key");
                }

                return InnerHashtable[key];
            }
            set {
                if (!(key is string)) {
                    throw new ArgumentException("key must be a string", "key");
                }

                InnerHashtable[key] = value;
            }
        }

        ICollection IDictionary.Values => InnerHashtable.Values;

        ICollection IDictionary.Keys => InnerHashtable.Keys;

        bool IDictionary.IsFixedSize => false;

        IDictionaryEnumerator IDictionary.GetEnumerator() => InnerHashtable.GetEnumerator();

        void IDictionary.Remove(object key) => InnerHashtable.Remove(key);

        bool IDictionary.Contains(object key) => InnerHashtable.Contains(key);

        void IDictionary.Add(object key, object value) {
            if (!(key is string)) {
                throw new ArgumentException("key must be a string", "key");
            }

            InnerHashtable.Add(key, value);
        }

        public override void Clear() => InnerHashtable.Clear();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerHashtable).GetEnumerator();

        public void Remove(string key) => InnerHashtable.Remove(key);
    }
}