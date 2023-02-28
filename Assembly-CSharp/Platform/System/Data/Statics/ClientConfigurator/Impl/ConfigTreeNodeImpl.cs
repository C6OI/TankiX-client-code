using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Library.ClientDataStructures.Impl;
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

        public void SetYaml(YamlNode yamlNode) {
            this.yamlNode = yamlNode;
        }

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

        public ICollection<ConfigTreeNode> GetChildren() {
            if (children.Count == 0) {
                return EmptyList<ConfigTreeNode>.Instance;
            }

            return children.Values.Select((Func<ConfigTreeNodeImpl, ConfigTreeNode>)(c => c)).ToList();
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

        public override string ToString() => string.Format("[{0}: ConfigPath={1} HasYaml={2}]", GetType().Name, ConfigPath, yamlNode != null);

        class PathHelper {
            int index;
            string[] pathParts;

            public void Init(string path) {
                pathParts = path.Split('/');
                TrimByGoBackPath();
                index = -1;
            }

            void TrimByGoBackPath() {
                int num = pathParts.Length - 1;

                while (pathParts[num].Equals("..")) {
                    num--;
                }

                int num2 = (pathParts.Length - 1 - num) * 2;

                if (num2 > 0) {
                    int num3 = pathParts.Length - num2;
                    string[] destinationArray = new string[num3];
                    Array.Copy(pathParts, destinationArray, num3);
                    pathParts = destinationArray;
                }
            }

            public bool HasNextPathPart() => index + 1 < pathParts.Length;

            public string GetNextPathPart() {
                index++;
                return pathParts[index];
            }
        }
    }
}