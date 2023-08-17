using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization {
    public sealed class ObjectDescriptor : IObjectDescriptor {
        public ObjectDescriptor(object value, Type type, Type staticType)
            : this(value, type, staticType, ScalarStyle.Any) { }

        public ObjectDescriptor(object value, Type type, Type staticType, ScalarStyle scalarStyle) {
            Value = value;

            if (type == null) {
                throw new ArgumentNullException("type");
            }

            Type = type;

            if (staticType == null) {
                throw new ArgumentNullException("staticType");
            }

            StaticType = staticType;
            ScalarStyle = scalarStyle;
        }

        public object Value { get; }

        public Type Type { get; }

        public Type StaticType { get; }

        public ScalarStyle ScalarStyle { get; }
    }
}