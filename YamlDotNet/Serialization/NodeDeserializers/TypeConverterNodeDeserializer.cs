using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class TypeConverterNodeDeserializer : INodeDeserializer {
        readonly IEnumerable<IYamlTypeConverter> converters;

        public TypeConverterNodeDeserializer(IEnumerable<IYamlTypeConverter> converters) {
            if (converters == null) {
                throw new ArgumentNullException("converters");
            }

            this.converters = converters;
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            IYamlTypeConverter yamlTypeConverter = converters.FirstOrDefault(c => c.Accepts(expectedType));

            if (yamlTypeConverter == null) {
                value = null;
                return false;
            }

            value = yamlTypeConverter.ReadYaml(reader.Parser, expectedType);
            return true;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey8B {
            internal Type expectedType;

            internal bool _003C_003Em__13D(IYamlTypeConverter c) => c.Accepts(expectedType);
        }
    }
}