using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class NonGenericListNodeDeserializer : INodeDeserializer {
        readonly IObjectFactory _objectFactory;

        public NonGenericListNodeDeserializer(IObjectFactory objectFactory) => _objectFactory = objectFactory;

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            if (!typeof(IList).IsAssignableFrom(expectedType)) {
                value = false;
                return false;
            }

            reader.Expect<SequenceStart>();
            IList list = (IList)_objectFactory.Create(expectedType);

            while (!reader.Accept<SequenceEnd>()) {
                object obj = nestedObjectDeserializer(reader, typeof(object));
                IValuePromise valuePromise = obj as IValuePromise;

                if (valuePromise == null) {
                    list.Add(obj);
                    continue;
                }

                int index = list.Count;
                list.Add(null);

                valuePromise.ValueAvailable += delegate(object v) {
                    list[index] = v;
                };
            }

            value = list;
            reader.Expect<SequenceEnd>();
            return true;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey87 {
            internal IList list;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey88 {
            internal Deserialize_003Ec__AnonStorey87 _003C_003Ef__ref_0024135;
            internal int index;

            internal void _003C_003Em__13B(object v) => _003C_003Ef__ref_0024135.list[index] = v;
        }
    }
}