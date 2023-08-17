using System;

namespace YamlDotNet.Serialization {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class YamlIgnoreAttribute : Attribute { }
}