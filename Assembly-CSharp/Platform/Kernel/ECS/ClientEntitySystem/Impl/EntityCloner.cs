using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityCloner {
        readonly Type newEntityComponentType = typeof(NewEntityComponent);

        public EntityInternal Clone(string name, EntityInternal entity, EntityBuilder entityBuilder) {
            entityBuilder.SetName(name);

            if (entity.TemplateAccessor.IsPresent()) {
                TemplateAccessor templateAccessor = entity.TemplateAccessor.Get();

                if (templateAccessor != null) {
                    TemplateDescription templateDescription = templateAccessor.TemplateDescription;
                    entityBuilder.SetTemplate(templateDescription.TemplateClass);

                    if (templateAccessor.HasConfigPath()) {
                        entityBuilder.SetConfig(templateAccessor.ConfigPath);
                    } else {
                        YamlNode yamlNode = templateAccessor.YamlNode;

                        if (yamlNode != null) {
                            entityBuilder.SetTemplateYamlNode(yamlNode);
                        }
                    }
                }
            }

            EntityInternal entityInternal = entityBuilder.Build();
            ICollection<Type> componentClasses = entity.ComponentClasses;

            foreach (Type item in componentClasses) {
                if (item != newEntityComponentType) {
                    entityInternal.AddComponent(entity.GetComponent(item));
                }
            }

            return entityInternal;
        }
    }
}