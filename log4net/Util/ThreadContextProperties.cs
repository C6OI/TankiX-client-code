using System;

namespace log4net.Util {
    public sealed class ThreadContextProperties : ContextPropertiesBase {
        [ThreadStatic] static PropertiesDictionary _dictionary;

        internal ThreadContextProperties() { }

        public override object this[string key] {
            get {
                if (_dictionary != null) {
                    return _dictionary[key];
                }

                return null;
            }
            set => GetProperties(true)[key] = value;
        }

        public void Remove(string key) {
            if (_dictionary != null) {
                _dictionary.Remove(key);
            }
        }

        public string[] GetKeys() {
            if (_dictionary != null) {
                return _dictionary.GetKeys();
            }

            return null;
        }

        public void Clear() {
            if (_dictionary != null) {
                _dictionary.Clear();
            }
        }

        internal PropertiesDictionary GetProperties(bool create) {
            if (_dictionary == null && create) {
                _dictionary = new PropertiesDictionary();
            }

            return _dictionary;
        }
    }
}