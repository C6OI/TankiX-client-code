using System;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ConfigComponentInfoBuilder : ComponentInfoBuilder {
        Type ComponentInfoBuilder.TemplateComponentInfoClass => typeof(ConfigComponentInfo);

        public bool IsAcceptable(MethodInfo componentMethod) {
            object[] customAttributes = componentMethod.GetCustomAttributes(typeof(PersistentConfig), false);
            return customAttributes.Length == 1;
        }

        public ComponentInfo Build(MethodInfo componentMethod, ComponentDescriptionImpl componentDescription) {
            object[] customAttributes = componentMethod.GetCustomAttributes(typeof(PersistentConfig), false);
            PersistentConfig persistentConfig = (PersistentConfig)customAttributes[0];
            string text = persistentConfig.value;

            if (text.Length == 0) {
                text = componentDescription.FieldName;
            }

            return new ConfigComponentInfo(text, persistentConfig.configOptional);
        }
    }
}