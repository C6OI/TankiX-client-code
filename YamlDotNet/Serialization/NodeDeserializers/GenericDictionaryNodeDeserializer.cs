using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class GenericDictionaryNodeDeserializer : INodeDeserializer {
        static readonly GenericStaticMethod deserializeHelperMethod =
            new(() => DeserializeHelper<object, object>(null, null, null, null));

        readonly IObjectFactory _objectFactory;

        public GenericDictionaryNodeDeserializer(IObjectFactory objectFactory) => _objectFactory = objectFactory;

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            Type implementedGenericInterface =
                ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IDictionary<,>));

            if (implementedGenericInterface == null) {
                value = false;
                return false;
            }

            reader.Expect<MappingStart>();
            value = _objectFactory.Create(expectedType);

            deserializeHelperMethod.Invoke(implementedGenericInterface.GetGenericArguments(),
                reader,
                expectedType,
                nestedObjectDeserializer,
                value);

            reader.Expect<MappingEnd>();
            return true;
        }

        static void DeserializeHelper<TKey, TValue>(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, IDictionary<TKey, TValue> result) {
            while (!reader.Accept<MappingEnd>()) {
                object key = nestedObjectDeserializer(reader, typeof(TKey));
                IValuePromise valuePromise = key as IValuePromise;
                object value = nestedObjectDeserializer(reader, typeof(TValue));
                IValuePromise valuePromise2 = value as IValuePromise;

                if (valuePromise == null) {
                    if (valuePromise2 == null) {
                        result[(TKey)key] = (TValue)value;
                        continue;
                    }

                    valuePromise2.ValueAvailable += delegate(object v) {
                        result[(TKey)key] = (TValue)v;
                    };

                    continue;
                }

                if (valuePromise2 == null) {
                    valuePromise.ValueAvailable += delegate(object v) {
                        result[(TKey)v] = (TValue)value;
                    };

                    continue;
                }

                bool hasFirstPart = false;

                valuePromise.ValueAvailable += delegate(object v) {
                    if (hasFirstPart) {
                        result[(TKey)v] = (TValue)value;
                    } else {
                        key = v;
                        hasFirstPart = true;
                    }
                };

                valuePromise2.ValueAvailable += delegate(object v) {
                    if (hasFirstPart) {
                        result[(TKey)key] = (TValue)v;
                    } else {
                        value = v;
                        hasFirstPart = true;
                    }
                };
            }
        }
    }
}