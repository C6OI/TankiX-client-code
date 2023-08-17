using System;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ConfigurationEntityIdCalculator {
        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        public static int Calculate(string path) {
            path = path.Replace("\\", "/");

            if (path.StartsWith("/", StringComparison.Ordinal)) {
                path = path.Substring(1);
            }

            if (path.EndsWith("/", StringComparison.Ordinal)) {
                path = path.Substring(0, path.Length - 1);
            }

            if (ConfigurationService.HasConfig(path)) {
                YamlNode config = ConfigurationService.GetConfig(path);

                if (config.HasValue("id")) {
                    return int.Parse(config.GetStringValue("id"));
                }
            }

            return path.GetHashCode();
        }
    }
}