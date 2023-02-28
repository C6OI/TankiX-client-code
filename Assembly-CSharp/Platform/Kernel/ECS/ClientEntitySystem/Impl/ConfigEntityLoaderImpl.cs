using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ConfigEntityLoaderImpl : ConfigEntityLoader {
        public static readonly string STARTUP_CONFIG_PATH = "service/configentityloader";

        static readonly string STARTUP_ROOT_NODE = "autoCreatedEntities";

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        [Inject] public static SharedEntityRegistry SharedEntityRegistry { get; set; }

        public void LoadEntities(Engine engine) {
            if (!ConfigurationService.HasConfig(STARTUP_CONFIG_PATH)) {
                return;
            }

            YamlNode config = ConfigurationService.GetConfig(STARTUP_CONFIG_PATH);

            IEnumerable<ConfigEntityInfo> enumerable = from node in config.GetChildListNodes(STARTUP_ROOT_NODE)
                                                       select node.ConvertTo<ConfigEntityInfo>();

            foreach (ConfigEntityInfo item in enumerable) {
                if (ConfigurationService.HasConfig(item.Path)) {
                    EntityInternal entityInternal = SharedEntityRegistry.CreateEntity(item.TemplateId, item.Path, GetConfigEntityId(item.Path));
                    SharedEntityRegistry.SetShared(entityInternal.Id);
                }
            }
        }

        int GetConfigEntityId(string path) => ConfigurationEntityIdCalculator.Calculate(path);
    }
}