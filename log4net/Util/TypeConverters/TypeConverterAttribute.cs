using System;

namespace log4net.Util.TypeConverters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface)]
    public sealed class TypeConverterAttribute : Attribute {
        public TypeConverterAttribute() { }

        public TypeConverterAttribute(string typeName) => ConverterTypeName = typeName;

        public TypeConverterAttribute(Type converterType) =>
            ConverterTypeName = SystemInfo.AssemblyQualifiedName(converterType);

        public string ConverterTypeName { get; set; }
    }
}