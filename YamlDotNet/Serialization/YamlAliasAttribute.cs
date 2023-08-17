using System;

namespace YamlDotNet.Serialization {
    [Obsolete("Please use YamlMember instead")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class YamlAliasAttribute : Attribute {
        public YamlAliasAttribute(string alias) => Alias = alias;

        public string Alias { get; set; }
    }
}