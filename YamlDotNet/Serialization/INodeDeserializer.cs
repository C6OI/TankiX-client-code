using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization {
    public interface INodeDeserializer {
        bool Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer,
            out object value);
    }
}