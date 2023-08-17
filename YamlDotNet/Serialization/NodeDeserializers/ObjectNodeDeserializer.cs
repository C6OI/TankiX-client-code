using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class ObjectNodeDeserializer : INodeDeserializer {
        readonly bool _ignoreUnmatched;

        readonly IObjectFactory _objectFactory;

        readonly ITypeInspector _typeDescriptor;

        public ObjectNodeDeserializer(IObjectFactory objectFactory, ITypeInspector typeDescriptor, bool ignoreUnmatched) {
            _objectFactory = objectFactory;
            _typeDescriptor = typeDescriptor;
            _ignoreUnmatched = ignoreUnmatched;
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            MappingStart mappingStart = reader.Allow<MappingStart>();

            if (mappingStart == null) {
                value = null;
                return false;
            }

            value = _objectFactory.Create(expectedType);

            while (!reader.Accept<MappingEnd>()) {
                Scalar scalar = reader.Expect<Scalar>();

                IPropertyDescriptor property =
                    _typeDescriptor.GetProperty(expectedType, null, scalar.Value, _ignoreUnmatched);

                if (property == null) {
                    reader.SkipThisAndNestedEvents();
                    continue;
                }

                object obj = nestedObjectDeserializer(reader, property.Type);
                IValuePromise valuePromise = obj as IValuePromise;

                if (valuePromise == null) {
                    object value2 = TypeConverter.ChangeType(obj, property.Type);
                    property.Write(value, value2);
                    continue;
                }

                object valueRef = value;

                valuePromise.ValueAvailable += delegate(object v) {
                    object value3 = TypeConverter.ChangeType(v, property.Type);
                    property.Write(valueRef, value3);
                };
            }

            reader.Expect<MappingEnd>();
            return true;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey89 {
            internal IPropertyDescriptor property;
        }

        [CompilerGenerated]
        sealed class Deserialize_003Ec__AnonStorey8A {
            internal Deserialize_003Ec__AnonStorey89 _003C_003Ef__ref_0024137;
            internal object valueRef;

            internal void _003C_003Em__13C(object v) {
                object value = TypeConverter.ChangeType(v, _003C_003Ef__ref_0024137.property.Type);
                _003C_003Ef__ref_0024137.property.Write(valueRef, value);
            }
        }
    }
}