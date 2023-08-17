using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;

namespace log4net.Util {
    [Serializable]
    public class ReadOnlyPropertiesDictionary : IEnumerable, ICollection, IDictionary, ISerializable {
        public ReadOnlyPropertiesDictionary() { }

        public ReadOnlyPropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary) {
            foreach (DictionaryEntry item in (IDictionary)propertiesDictionary) {
                InnerHashtable.Add(item.Key, item.Value);
            }
        }

        protected ReadOnlyPropertiesDictionary(SerializationInfo info, StreamingContext context) {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            while (enumerator.MoveNext()) {
                SerializationEntry current = enumerator.Current;
                InnerHashtable[XmlConvert.DecodeName(current.Name)] = current.Value;
            }
        }

        public virtual object this[string key] {
            get => InnerHashtable[key];
            set => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        protected Hashtable InnerHashtable { get; } = new();

        bool ICollection.IsSynchronized => InnerHashtable.IsSynchronized;

        object ICollection.SyncRoot => InnerHashtable.SyncRoot;

        public int Count => InnerHashtable.Count;

        void ICollection.CopyTo(Array array, int index) => InnerHashtable.CopyTo(array, index);

        bool IDictionary.IsReadOnly => true;

        object IDictionary.this[object key] {
            get {
                if (!(key is string)) {
                    throw new ArgumentException("key must be a string");
                }

                return InnerHashtable[key];
            }
            set => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        ICollection IDictionary.Values => InnerHashtable.Values;

        ICollection IDictionary.Keys => InnerHashtable.Keys;

        bool IDictionary.IsFixedSize => InnerHashtable.IsFixedSize;

        IDictionaryEnumerator IDictionary.GetEnumerator() => InnerHashtable.GetEnumerator();

        void IDictionary.Remove(object key) =>
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");

        bool IDictionary.Contains(object key) => InnerHashtable.Contains(key);

        void IDictionary.Add(object key, object value) =>
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");

        public virtual void Clear() =>
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerHashtable).GetEnumerator();

        [PermissionSet(SecurityAction.Demand,
            XML =
                "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            foreach (DictionaryEntry item in InnerHashtable) {
                string text = item.Key as string;
                object value = item.Value;

                if (text != null && value != null && value.GetType().IsSerializable) {
                    info.AddValue(XmlConvert.EncodeLocalName(text), value);
                }
            }
        }

        public string[] GetKeys() {
            string[] array = new string[InnerHashtable.Count];
            InnerHashtable.Keys.CopyTo(array, 0);
            return array;
        }

        public bool Contains(string key) => InnerHashtable.Contains(key);
    }
}