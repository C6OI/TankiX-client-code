using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class ConfigComponentConstructor : AbstractTemplateComponentConstructor {
        protected override bool IsAcceptable(ComponentDescription componentDescription, EntityInternal entity) =>
            componentDescription.IsInfoPresent(typeof(ConfigComponentInfo));

        protected internal override Component GetComponentInstance(ComponentDescription componentDescription,
            EntityInternal entity) {
            YamlNode yamlNode = entity.TemplateAccessor.Get().YamlNode;
            ConfigComponentInfo info = componentDescription.GetInfo<ConfigComponentInfo>();
            string keyName = info.KeyName;

            if (info.ConfigOptional && !yamlNode.HasValue(keyName)) {
                return (Component)Activator.CreateInstance(componentDescription.ComponentType);
            }

            YamlNode childNode = yamlNode.GetChildNode(keyName);
            return (Component)childNode.ConvertTo(componentDescription.ComponentType);
        }
    }
}