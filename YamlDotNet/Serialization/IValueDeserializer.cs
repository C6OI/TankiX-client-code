using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization {
    public interface IValueDeserializer {
        object DeserializeValue(EventReader reader, Type expectedType, SerializerState state,
            IValueDeserializer nestedObjectDeserializer);
    }
}