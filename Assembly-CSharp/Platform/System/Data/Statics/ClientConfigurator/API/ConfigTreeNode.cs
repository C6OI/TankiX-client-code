using System.Collections.Generic;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public interface ConfigTreeNode {
        string ConfigPath { get; }

        bool HasYaml();

        void SetYaml(YamlNode yamlNode);

        YamlNode GetYaml();

        ConfigTreeNode FindNode(string path);

        ConfigTreeNode FindOrCreateNode(string configPath);

        ICollection<ConfigTreeNode> GetChildren();
    }
}