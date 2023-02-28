using System;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public class ConfigNodeDoesntContainYamlException : Exception {
        public ConfigNodeDoesntContainYamlException(ConfigTreeNode treeNode)
            : base("node: " + treeNode) { }
    }
}