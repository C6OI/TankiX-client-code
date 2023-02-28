using System;
using System.Collections.Generic;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigurationServiceFake : ConfigurationService {
        readonly IDictionary<string, YamlNode> nodes = new Dictionary<string, YamlNode>();

        public Action ErrorHandler { private get; set; }

        public bool HasConfig(string path) => nodes.ContainsKey(path);

        public YamlNode GetConfig(string path) => nodes[path];

        public YamlNode GetConfigOrNull(string path) {
            YamlNode value;
            nodes.TryGetValue(path, out value);
            return value;
        }

        public List<string> GetPathsByWildcard(string pathWithWildcard) => new();

        public virtual IEnumerable<ConfigTreeNode> GetConfigLocations(string path) => Collections.EmptyList<ConfigTreeNode>();

        public void AddConfig(string path, YamlNode yamlNode) {
            nodes[path] = yamlNode;
        }
    }
}