using System.Collections.Generic;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigTreeNodeImpl : ConfigTreeNode {
        static readonly PathHelper pathHelper = new();

        readonly Dictionary<string, ConfigTreeNodeImpl> children;

        YamlNode yamlNode;

        public ConfigTreeNodeImpl() {
            children = new Dictionary<string, ConfigTreeNodeImpl>();
            ConfigPath = string.Empty;
        }

        public ConfigTreeNodeImpl(string configPath) {
            children = new Dictionary<string, ConfigTreeNodeImpl>();
            ConfigPath = configPath;
        }

        public string ConfigPath { get; protected set; }

        public bool HasYaml() => yamlNode != null;

        public YamlNode GetYaml() => yamlNode;

        public void SetYaml(YamlNode yamlNode) => this.yamlNode = yamlNode;

        public ConfigTreeNode FindNode(string path) {
            if (string.IsNullOrEmpty(path)) {
                return null;
            }

            path = path.Trim('/');
            pathHelper.Init(path);
            ConfigTreeNodeImpl configTreeNodeImpl = this;

            while (pathHelper.HasNextPathPart()) {
                ConfigTreeNodeImpl value;

                if (configTreeNodeImpl.children.TryGetValue(pathHelper.GetNextPathPart(), out value)) {
                    configTreeNodeImpl = value;
                    continue;
                }

                return null;
            }

            return configTreeNodeImpl;
        }

        public ConfigTreeNode FindOrCreateNode(string configPath) {
            if (string.IsNullOrEmpty(configPath)) {
                return this;
            }

            configPath = configPath.Trim('/');
            pathHelper.Init(configPath);
            ConfigTreeNodeImpl configTreeNodeImpl = this;

            while (pathHelper.HasNextPathPart()) {
                string nextPathPart = pathHelper.GetNextPathPart();
                ConfigTreeNodeImpl value;

                if (!configTreeNodeImpl.children.TryGetValue(nextPathPart, out value)) {
                    value = new ConfigTreeNodeImpl(nextPathPart);
                    configTreeNodeImpl.children.Add(nextPathPart, value);
                }

                configTreeNodeImpl = value;
            }

            return configTreeNodeImpl;
        }

        public void Add(ConfigTreeNodeImpl configTreeNode) {
            if (ConfigPath != configTreeNode.ConfigPath) {
                TryAddAsChild(configTreeNode.ConfigPath, configTreeNode);
                return;
            }

            foreach (KeyValuePair<string, ConfigTreeNodeImpl> child in configTreeNode.children) {
                TryAddAsChild(child.Key, child.Value);
            }
        }

        void TryAddAsChild(string configName, ConfigTreeNodeImpl config) {
            if (children.ContainsKey(configName)) {
                children[configName].Add(config);
            } else {
                children.Add(configName, config);
            }
        }

        public override string ToString() =>
            string.Format("[{0}: ConfigPath={1} HasYaml={2}]", GetType().Name, ConfigPath, yamlNode != null);

        class PathHelper {
            int index;
            string[] pathParts;

            public void Init(string path) {
                pathParts = path.Split('/');
                index = -1;
            }

            public bool HasNextPathPart() => index + 1 < pathParts.Length;

            public string GetNextPathPart() {
                index++;
                return pathParts[index];
            }
        }
    }
}