using System.Collections;

namespace log4net.Util {
    public sealed class CompositeProperties {
        readonly ArrayList m_nestedProperties = new();
        PropertiesDictionary m_flattened;

        internal CompositeProperties() { }

        public object this[string key] {
            get {
                if (m_flattened != null) {
                    return m_flattened[key];
                }

                foreach (ReadOnlyPropertiesDictionary nestedProperty in m_nestedProperties) {
                    if (nestedProperty.Contains(key)) {
                        return nestedProperty[key];
                    }
                }

                return null;
            }
        }

        public void Add(ReadOnlyPropertiesDictionary properties) {
            m_flattened = null;
            m_nestedProperties.Add(properties);
        }

        public PropertiesDictionary Flatten() {
            if (m_flattened == null) {
                m_flattened = new PropertiesDictionary();
                int num = m_nestedProperties.Count;

                while (--num >= 0) {
                    ReadOnlyPropertiesDictionary readOnlyPropertiesDictionary =
                        (ReadOnlyPropertiesDictionary)m_nestedProperties[num];

                    foreach (DictionaryEntry item in (IDictionary)readOnlyPropertiesDictionary) {
                        m_flattened[(string)item.Key] = item.Value;
                    }
                }
            }

            return m_flattened;
        }
    }
}