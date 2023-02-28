using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TemplateAccessor {
        readonly YamlNode yamlNode;

        public string configPath;

        public TemplateAccessor(TemplateDescription templateDescription, string configPath)
            : this(templateDescription) => this.configPath = configPath;

        public TemplateAccessor(TemplateDescription templateDescription, YamlNode yamlNode)
            : this(templateDescription) => this.yamlNode = yamlNode;

        TemplateAccessor(TemplateDescription templateDescription) => TemplateDescription = templateDescription;

        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        public virtual TemplateDescription TemplateDescription { get; }

        public YamlNode YamlNode => !HasConfigPath() ? yamlNode : ConfiguratorService.GetConfig(ConfigPath);

        public string ConfigPath {
            get {
                if (!HasConfigPath()) {
                    throw new CannotAccessPathForTemplate(TemplateDescription.TemplateClass);
                }

                return configPath;
            }
            set => configPath = value;
        }

        public bool HasConfigPath() => !string.IsNullOrEmpty(configPath);
    }
}