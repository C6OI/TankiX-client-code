using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class NonGenericDictionaryNodeDeserializer : INodeDeserializer {
        readonly IObjectFactory _objectFactory;

        public NonGenericDictionaryNodeDeserializer(IObjectFactory objectFactory) => _objectFactory = objectFactory;

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            if (!typeof(IDictionary).IsAssignableFrom(expectedType)) {
                value = false;
                return false;
            }

            reader.Expect<MappingStart>();
            IDictionary dictionary = (IDictionary)_objectFactory.Create(expectedType);

            while (!reader.Accept<MappingEnd>()) {
                object key = nestedObjectDeserializer(reader, typeof(object));
                IValuePromise valuePromise = key as IValuePromise;
                object keyValue = nestedObjectDeserializer(reader, typeof(object));
                IValuePromise valuePromise2 = keyValue as IValuePromise;

                if (valuePromise == null) {
                    if (valuePromise2 == null) {
                        dictionary.Add(key, keyValue);
                        continue;
                    }

                    valuePromise2.ValueAvailable += delegate(object v) {
                        dictionary.Add(key, v);
                    };

                    continue;
                }

                if (valuePromise2 == null) {
                    valuePromise.ValueAvailable += delegate(object v) {
                        dictionary.Add(v, keyValue);
                    };

                    continue;
                }

                bool hasFirstPart = false;

                valuePromise.ValueAvailable += delegate(object v) {
                    if (hasFirstPart) {
                        dictionary.Add(v, keyValue);
                    } else {
                        key = v;
                        hasFirstPart = true;
                    }
                };

                valuePromise2.ValueAvailable += delegate(object v) {
                    if (hasFirstPart) {
                        dictionary.Add(key, v);
                    } else {
                        keyValue = v;
                        hasFirstPart = true;
                    }
                };
            }

            value = dictionary;
            reader.Expect<MappingEnd>();
            return true;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey84 {
            internal IDictionary dictionary;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey85 {
            internal Deserialize_003Ec__AnonStorey84 _003C_003Ef__ref_0024132;
            internal object key;

            internal object keyValue;

            internal void _003C_003Em__137(object v) => _003C_003Ef__ref_0024132.dictionary.Add(key, v);

            internal void _003C_003Em__138(object v) => _003C_003Ef__ref_0024132.dictionary.Add(v, keyValue);
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey86 {
            internal Deserialize_003Ec__AnonStorey84 _003C_003Ef__ref_0024132;

            internal Deserialize_003Ec__AnonStorey85 _003C_003Ef__ref_0024133;
            internal bool hasFirstPart;

            internal void _003C_003Em__139(object v) {
                if (hasFirstPart) {
                    _003C_003Ef__ref_0024132.dictionary.Add(v, _003C_003Ef__ref_0024133.keyValue);
                    return;
                }

                _003C_003Ef__ref_0024133.key = v;
                hasFirstPart = true;
            }

            internal void _003C_003Em__13A(object v) {
                if (hasFirstPart) {
                    _003C_003Ef__ref_0024132.dictionary.Add(_003C_003Ef__ref_0024133.key, v);
                    return;
                }

                _003C_003Ef__ref_0024133.keyValue = v;
                hasFirstPart = true;
            }
        }
    }
}