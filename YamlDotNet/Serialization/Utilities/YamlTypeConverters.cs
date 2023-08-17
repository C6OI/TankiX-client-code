using System.Collections.Generic;
using YamlDotNet.Serialization.Converters;

namespace YamlDotNet.Serialization.Utilities {
    static class YamlTypeConverters {
        public static IEnumerable<IYamlTypeConverter> BuiltInConverters { get; } = new IYamlTypeConverter[1] {
            new GuidConverter()
        };
    }
}