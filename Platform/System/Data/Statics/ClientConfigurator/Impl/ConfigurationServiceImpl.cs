using System.Collections.Generic;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Platform.System.Data.Statics.ClientYaml.Impl;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigurationServiceImpl : ConfigurationService {
        public static readonly YamlNode EMPTY_YAML_NODE = new YamlNodeImpl(new Dictionary<object, object>());

        public static readonly string CONFIG_FILE = "public";

        ConfigTreeNode rootConfigNode;

        public ConfigurationServiceImpl() => rootConfigNode = new ConfigTreeNodeImpl(string.Empty);

        public bool HasConfig(string path) {
            ConfigTreeNode configTreeNode = rootConfigNode.FindNode(path);
            return configTreeNode != null && configTreeNode.HasYaml();
        }

        public YamlNode GetConfig(string path) {
            ConfigTreeNode configTreeNode = rootConfigNode.FindNode(path);

            if (configTreeNode == null) {
                throw new ConfigWasNotFoundException(path);
            }

            if (!configTreeNode.HasYaml()) {
                throw new ConfigNodeDoesntContainYamlException(configTreeNode);
            }

            return configTreeNode.GetYaml();
        }

        public void SetRootConfigNode(ConfigTreeNode configTreeNode) => rootConfigNode = configTreeNode;
    }
}