using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class NullNodeDeserializer : INodeDeserializer {
        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            value = null;
            NodeEvent nodeEvent = reader.Peek<NodeEvent>();
            bool flag = nodeEvent != null && NodeIsNull(nodeEvent);

            if (flag) {
                reader.SkipThisAndNestedEvents();
            }

            return flag;
        }

        bool NodeIsNull(NodeEvent nodeEvent) {
            if (nodeEvent.Tag == "tag:yaml.org,2002:null") {
                return true;
            }

            Scalar scalar = nodeEvent as Scalar;

            if (scalar == null || scalar.Style != ScalarStyle.Plain) {
                return false;
            }

            string value = scalar.Value;

            if (value == string.Empty) {
                goto IL_0086;
            }

            switch (value) {
                case "~":
                case "null":
                case "Null":
                    goto IL_0086;
            }

            int result = value == "NULL" ? 1 : 0;
            IL_0087:
            return (byte)result != 0;

            IL_0086:
            result = 1;
            goto IL_0087;
        }
    }
}