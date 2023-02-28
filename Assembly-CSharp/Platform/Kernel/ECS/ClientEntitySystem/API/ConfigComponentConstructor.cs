using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class ConfigComponentConstructor : AbstractTemplateComponentConstructor {
        protected override bool IsAcceptable(ComponentDescription componentDescription, EntityInternal entity) => componentDescription.IsInfoPresent(typeof(ConfigComponentInfo));

        protected internal override Component GetComponentInstance(ComponentDescription componentDescription, EntityInternal entity) {
            TemplateAccessor templateAccessor = entity.TemplateAccessor.Get();
            YamlNode yamlNode = templateAccessor.YamlNode;
            ConfigComponentInfo info = componentDescription.GetInfo<ConfigComponentInfo>();
            string keyName = info.KeyName;

            if (info.ConfigOptional && !yamlNode.HasValue(keyName)) {
                return (Component)Activator.CreateInstance(componentDescription.ComponentType);
            }

            try {
                YamlNode childNode = yamlNode.GetChildNode(keyName);
                return (Component)childNode.ConvertTo(componentDescription.ComponentType);
            } catch (Exception innerException) {
                string text = !templateAccessor.HasConfigPath() ? yamlNode.ToString() : templateAccessor.ConfigPath;

                throw new Exception(
                    string.Concat("Error deserializing component ", componentDescription.ComponentType, " from configs, entity=", entity, ", key=", keyName, ", pathOrNode=", text),
                    innerException);
            }
        }
    }
}