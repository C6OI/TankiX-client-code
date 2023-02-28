using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ConfigComponentInfo : ComponentInfo {
        public ConfigComponentInfo(string keyName, bool configOptional) {
            KeyName = keyName;
            ConfigOptional = configOptional;
        }

        public string KeyName { get; set; }

        public bool ConfigOptional { get; }
    }
}