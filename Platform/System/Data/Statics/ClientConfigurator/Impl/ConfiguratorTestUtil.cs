using System.IO;
using Platform.System.Data.Statics.ClientConfigurator.API;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfiguratorTestUtil {
        public static readonly string IDE_CONFIG_PATH =
            "../../../../ServerGame/config".Replace('/', Path.DirectorySeparatorChar);

        public static readonly string MAVEN_CONFIG_PATH =
            "../../../../assembly/ServerGame/config".Replace('/', Path.DirectorySeparatorChar);

        public static void SetDefaultConfigs(ConfigurationServiceImpl configurationService) {
            FileSystemConfigsImporter fileSystemConfigsImporter = new();
            string path;

            if (Directory.Exists(MAVEN_CONFIG_PATH)) {
                path = MAVEN_CONFIG_PATH;
            } else {
                if (!Directory.Exists(IDE_CONFIG_PATH)) {
                    throw new ConfigsNotFoundException("Configs directory was not found. Path for maven: " +
                                                       MAVEN_CONFIG_PATH +
                                                       ". Path for IDE: " +
                                                       IDE_CONFIG_PATH +
                                                       ".");
                }

                path = IDE_CONFIG_PATH;
            }

            ConfigTreeNodeImpl rootConfigNode =
                fileSystemConfigsImporter.Import<ConfigTreeNodeImpl>(path, new ConfigurationProfileImpl());

            configurationService.SetRootConfigNode(rootConfigNode);
        }

        public static void SetTestConfigs(ConfigurationServiceImpl configurationService, string path) {
            if (Directory.Exists(path)) {
                FileSystemConfigsImporter fileSystemConfigsImporter = new();

                ConfigTreeNodeImpl rootConfigNode =
                    fileSystemConfigsImporter.Import<ConfigTreeNodeImpl>(path, new ConfigurationProfileImpl());

                configurationService.SetRootConfigNode(rootConfigNode);
                return;
            }

            throw new ConfigsNotFoundException("Configs directory '" + path + "' was not found.");
        }
    }
}