namespace log4net.Util {
    public sealed class GlobalContextProperties : ContextPropertiesBase {
        readonly object m_syncRoot = new();
        volatile ReadOnlyPropertiesDictionary m_readOnlyProperties = new();

        internal GlobalContextProperties() { }

        public override object this[string key] {
            get => m_readOnlyProperties[key];
            set {
                lock (m_syncRoot) {
                    PropertiesDictionary propertiesDictionary = new(m_readOnlyProperties);
                    propertiesDictionary[key] = value;
                    m_readOnlyProperties = new ReadOnlyPropertiesDictionary(propertiesDictionary);
                }
            }
        }

        public void Remove(string key) {
            lock (m_syncRoot) {
                if (m_readOnlyProperties.Contains(key)) {
                    PropertiesDictionary propertiesDictionary = new(m_readOnlyProperties);
                    propertiesDictionary.Remove(key);
                    m_readOnlyProperties = new ReadOnlyPropertiesDictionary(propertiesDictionary);
                }
            }
        }

        public void Clear() {
            lock (m_syncRoot) {
                m_readOnlyProperties = new ReadOnlyPropertiesDictionary();
            }
        }

        internal ReadOnlyPropertiesDictionary GetReadOnlyProperties() => m_readOnlyProperties;
    }
}