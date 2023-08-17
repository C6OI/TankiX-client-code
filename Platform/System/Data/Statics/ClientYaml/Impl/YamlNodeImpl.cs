using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.System.Data.Statics.ClientYaml.Impl {
    public class YamlNodeImpl : YamlNode {
        internal Dictionary<object, object> innerDictionary;

        public Dictionary<string, YamlNode> keyToChildNode = new();

        public Dictionary<Type, object> typeToPrototypeCache = new(new Comparer());

        public YamlNodeImpl(Dictionary<object, object> innerDictionary) => this.innerDictionary = innerDictionary;

        [Inject] public static YamlService YamlService { get; set; }

        public YamlNode GetChildNode(string key) {
            YamlNode value;

            if (!keyToChildNode.TryGetValue(key, out value)) {
                value = new YamlNodeImpl(CastValueTo<Dictionary<object, object>>(key));
                keyToChildNode.Add(key, value);
            }

            return value;
        }

        public List<T> GetList<T>(string key) => GetListContentsOf<T>(key).ToList();

        public List<YamlNode> GetChildListNodes(string key) => (from x in GetListContentsOf<Dictionary<object, object>>(key)
                                                                select new YamlNodeImpl(x)).Cast<YamlNode>().ToList();

        public List<string> GetChildListValues(string key) => GetListContentsOf<string>(key).ToList();

        public string GetStringValue(string key) => CastValueTo<string>(key);

        public object GetValue(string key) => innerDictionary[key];

        public bool HasValue(string key) => innerDictionary.ContainsKey(key);

        public T ConvertTo<T>() => (T)ConvertTo(typeof(T));

        public object ConvertTo(Type t) {
            object objectPrototypeForType = GetObjectPrototypeForType(t);
            return CloneObjectUtil.CloneObject(objectPrototypeForType);
        }

        IEnumerable<T> GetListContentsOf<T>(string key) {
            List<object> source = CastValueTo<List<object>>(key);

            if (!source.All(x => x is T)) {
                throw new WrongYamlStructureException("element of " + key, typeof(T), typeof(object));
            }

            return source.Cast<T>();
        }

        object GetObjectPrototypeForType(Type t) {
            object value;
            typeToPrototypeCache.TryGetValue(t, out value);

            if (value == null) {
                value = YamlService.Load(this, t);
                typeToPrototypeCache.Add(t, value);
            }

            return value;
        }

        public void Merge(YamlNodeImpl yamlNode) => MergeDictionary(innerDictionary, yamlNode.innerDictionary);

        public void MergeDictionary(IDictionary destination, IDictionary source) {
            foreach (DictionaryEntry item in source) {
                destination[item.Key] = MergeValue(item.Key, destination[item.Key], item.Value);
            }
        }

        object MergeValue(object key, object destValue, object sourceValue) {
            if (destValue == null) {
                return sourceValue;
            }

            if (destValue.GetType() != sourceValue.GetType()) {
                throw new MergingYamlMismatchException((string)key, destValue.GetType(), sourceValue.GetType());
            }

            if (destValue is IDictionary) {
                MergeDictionary((IDictionary)destValue, (IDictionary)sourceValue);
            } else {
                if (!(destValue is IList)) {
                    return sourceValue;
                }

                MergeList((IList)destValue, (IList)sourceValue);
            }

            return destValue;
        }

        void MergeList(IList destination, IList source) {
            foreach (object item in source) {
                destination.Add(item);
            }
        }

        public T CastValueTo<T>(string key) {
            if (!innerDictionary.ContainsKey(key)) {
                throw new UnknownYamlKeyException(key);
            }

            object obj = innerDictionary[key];
            CheckType(key, typeof(T), obj);
            return (T)obj;
        }

        void CheckType(string key, Type type, object value) {
            if (type != null && !type.IsInstanceOfType(value)) {
                throw new WrongYamlStructureException(key, type, value.GetType());
            }
        }

        public override string ToString() {
            StringBuilder stringBuilder = new();

            foreach (KeyValuePair<object, object> item in innerDictionary) {
                stringBuilder.AppendFormat("{0}: {1}, ", item.Key, item.Value);
            }

            return stringBuilder.ToString().TrimEnd(",".ToCharArray());
        }

        public class Comparer : IEqualityComparer<Type> {
            public bool Equals(Type x, Type y) => x.FullName.Equals(y.FullName);

            public int GetHashCode(Type obj) => obj.GetHashCode();
        }
    }
}