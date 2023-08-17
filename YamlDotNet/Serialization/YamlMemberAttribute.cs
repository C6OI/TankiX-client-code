using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization {
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class YamlMemberAttribute : Attribute {
        public YamlMemberAttribute() => ScalarStyle = ScalarStyle.Any;

        public YamlMemberAttribute(Type serializeAs)
            : this() => SerializeAs = serializeAs;

        public Type SerializeAs { get; set; }

        public int Order { get; set; }

        public string Alias { get; set; }

        public ScalarStyle ScalarStyle { get; set; }
    }
}