using YamlDotNet.Serialization;

namespace Platform.System.Data.Statics.ClientYaml.Impl {
    public class PascalToCamelCaseNamingConvertion : INamingConvention {
        public string Apply(string value) => char.ToLower(value[0]) + value.Substring(1);
    }
}