using System;
using System.Collections.Generic;

namespace Platform.Library.ClientProtocol.API {
    public class CodecInfoWithAttributes {
        readonly Dictionary<Type, object> attributes;

        public CodecInfoWithAttributes() => attributes = new Dictionary<Type, object>();

        public CodecInfoWithAttributes(CodecInfo info)
            : this() => Info = info;

        public CodecInfoWithAttributes(Type type, bool optional, bool varied)
            : this(new CodecInfo(type, optional, varied)) { }

        public CodecInfo Info { get; }

        public T GetAttribute<T>() where T : Attribute => (T)attributes[typeof(T)];

        public bool IsAttributePresent<T>() where T : Attribute => attributes.ContainsKey(typeof(T));

        public void AddAttribute(object attribute) {
            attributes.Add(attribute.GetType(), attribute);
        }
    }
}