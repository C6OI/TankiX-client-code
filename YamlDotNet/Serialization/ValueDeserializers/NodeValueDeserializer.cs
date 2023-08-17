using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers {
    public sealed class NodeValueDeserializer : IValueDeserializer {
        readonly IList<INodeDeserializer> deserializers;

        readonly IList<INodeTypeResolver> typeResolvers;

        public NodeValueDeserializer(IList<INodeDeserializer> deserializers, IList<INodeTypeResolver> typeResolvers) {
            if (deserializers == null) {
                throw new ArgumentNullException("deserializers");
            }

            this.deserializers = deserializers;

            if (typeResolvers == null) {
                throw new ArgumentNullException("typeResolvers");
            }

            this.typeResolvers = typeResolvers;
        }

        public object DeserializeValue(EventReader reader, Type expectedType, SerializerState state,
            IValueDeserializer nestedObjectDeserializer) {
            NodeEvent nodeEvent = reader.Peek<NodeEvent>();
            Type typeFromEvent = GetTypeFromEvent(nodeEvent, expectedType);

            try {
                foreach (INodeDeserializer deserializer in deserializers) {
                    object value;

                    if (deserializer.Deserialize(reader,
                            typeFromEvent,
                            (r, t) => nestedObjectDeserializer.DeserializeValue(r, t, state, nestedObjectDeserializer),
                            out value)) {
                        return value;
                    }
                }
            } catch (YamlException) {
                throw;
            } catch (Exception innerException) {
                throw new YamlException(nodeEvent.Start, nodeEvent.End, "Exception during deserialization", innerException);
            }

            throw new YamlException(nodeEvent.Start,
                nodeEvent.End,
                string.Format("No node deserializer was able to deserialize the node into type {0}",
                    expectedType.AssemblyQualifiedName));
        }

        Type GetTypeFromEvent(NodeEvent nodeEvent, Type currentType) {
            foreach (INodeTypeResolver typeResolver in typeResolvers) {
                if (typeResolver.Resolve(nodeEvent, ref currentType)) {
                    break;
                }
            }

            return currentType;
        }
    }
}