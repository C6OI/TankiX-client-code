using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class ArrayNodeDeserializer : INodeDeserializer {
        static readonly GenericStaticMethod _deserializeHelper = new(() => DeserializeHelper<object>(null, null, null));

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            if (!expectedType.IsArray) {
                value = false;
                return false;
            }

            value = _deserializeHelper.Invoke(new Type[1] { expectedType.GetElementType() },
                reader,
                expectedType,
                nestedObjectDeserializer);

            return true;
        }

        static TItem[] DeserializeHelper<TItem>(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer) {
            List<TItem> list = new();
            GenericCollectionNodeDeserializer.DeserializeHelper(reader, expectedType, nestedObjectDeserializer, list);
            return list.ToArray();
        }
    }
}