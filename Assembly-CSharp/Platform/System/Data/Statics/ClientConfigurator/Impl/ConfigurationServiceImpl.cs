using System.Collections.Generic;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Platform.System.Data.Statics.ClientYaml.Impl;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigurationServiceImpl : ConfigurationService {
        public static readonly YamlNode EMPTY_YAML_NODE = new YamlNodeImpl(new Dictionary<object, object>());

        public static readonly string CONFIG_FILE = "public";

        public readonly Dictionary<string, YamlNodeImpl> cache = new();

        ConfigTreeNode root;

        public ConfigurationServiceImpl() => root = new ConfigTreeNodeImpl(string.Empty);

        public bool HasConfig(string path) => GetConfigOrNull(path) != null;

        public YamlNode GetConfig(string path) {
            YamlNode configOrNull = GetConfigOrNull(path);

            if (configOrNull == null) {
                throw new ConfigWasNotFoundException(path);
            }

            return configOrNull;
        }

        public YamlNode GetConfigOrNull(string path) {
            YamlNodeImpl value;

            if (cache.TryGetValue(path, out value)) {
                return value;
            }

            ConfigTreeNode configTreeNode = root.FindNode(path);
            value = configTreeNode == null ? null : (YamlNodeImpl)configTreeNode.GetYaml();
            cache[path] = value;
            return value;
        }

        public List<string> GetPathsByWildcard(string pathWithWildcard) {
            List<string> list = new();

            if (pathWithWildcard.EndsWith("*")) {
                string text = pathWithWildcard.Substring(0, pathWithWildcard.Length - 2);
                ConfigTreeNode parentNode = root.FindNode(text);
                CollectAllLeafPaths(parentNode, text, list);
            }

            return list;
        }

        public void SetRootConfigNode(ConfigTreeNode configTreeNode) {
            root = configTreeNode;
        }

        public void CacheAllPaths() {
            CacheAllPaths(root, string.Empty, cache);
        }

        static void CollectAllLeafPaths(ConfigTreeNode parentNode, string parentPath, List<string> paths) {
            if (parentNode == null) {
                return;
            }

            foreach (ConfigTreeNode child in parentNode.GetChildren()) {
                string text = parentPath + "/" + child.ConfigPath;

                if (child.GetChildren().Count > 0) {
                    CollectAllLeafPaths(child, text, paths);
                } else {
                    paths.Add(text);
                }
            }
        }

        static void CacheAllPaths(ConfigTreeNode node, string parentPath, Dictionary<string, YamlNodeImpl> nodes) {
            foreach (ConfigTreeNode child in node.GetChildren()) {
                string text = !string.IsNullOrEmpty(parentPath) ? parentPath + "/" + child.ConfigPath : child.ConfigPath;

                if (child.HasYaml()) {
                    nodes.Add(text, (YamlNodeImpl)child.GetYaml());
                }

                CacheAllPaths(child, text, nodes);
            }
        }
    }
}