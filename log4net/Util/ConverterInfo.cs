using System;

namespace log4net.Util {
    public sealed class ConverterInfo {
        public string Name { get; set; }

        public Type Type { get; set; }

        public PropertiesDictionary Properties { get; } = new();

        public void AddProperty(PropertyEntry entry) => Properties[entry.Key] = entry.Value;
    }
}