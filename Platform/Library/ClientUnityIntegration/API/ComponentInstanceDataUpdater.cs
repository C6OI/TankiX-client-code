using System;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.System.Data.Statics.ClientYaml.API;
using Platform.System.Data.Statics.ClientYaml.Impl;
using YamlDotNet.Serialization;

namespace Platform.Library.ClientUnityIntegration.API {
    public class ComponentInstanceDataUpdater {
        static readonly INamingConvention nameConvertor = new PascalToCamelCaseNamingConvertion();

        public static bool Update(EntityInternal entity, Component component, INamingConvention nameConvertor = null) {
            INamingConvention namingConvention = ComponentInstanceDataUpdater.nameConvertor;

            if (entity.TemplateAccessor.IsPresent()) {
                TemplateAccessor templateAccessor = entity.TemplateAccessor.Get();

                if (!templateAccessor.HasConfigPath()) {
                    return false;
                }

                if (nameConvertor != null) {
                    namingConvention = nameConvertor;
                }

                Type type = component.GetType();

                if (!templateAccessor.TemplateDescription.IsComponentDescriptionPresent(type)) {
                    return false;
                }

                ComponentDescription componentDescription =
                    templateAccessor.TemplateDescription.GetComponentDescription(type);

                if (!componentDescription.IsInfoPresent(typeof(ConfigComponentInfo))) {
                    return false;
                }

                string keyName = componentDescription.GetInfo<ConfigComponentInfo>().KeyName;

                if (!templateAccessor.YamlNode.HasValue(keyName)) {
                    return false;
                }

                UpdateComponentData(component, templateAccessor.YamlNode.GetChildNode(keyName), namingConvention);
                return true;
            }

            return false;
        }

        static void UpdateComponentData(Component component, YamlNode componentYamlNode, INamingConvention nameConvertor) {
            PropertyInfo[] properties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] array = properties;

            foreach (PropertyInfo propertyInfo in array) {
                string key = nameConvertor.Apply(propertyInfo.Name);

                if (componentYamlNode.HasValue(key) && propertyInfo.CanWrite) {
                    propertyInfo.SetValue(component, componentYamlNode.GetValue(key), null);
                }
            }
        }
    }
}