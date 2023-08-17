using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityCloner {
        readonly Type newEntityComponentType = typeof(NewEntityComponent);

        public EntityInternal Clone(string name, EntityInternal entity, EntityBuilder entityBuilder) {
            entityBuilder.SetName(name);
            Optional<TemplateAccessor> templateAccessor = entity.TemplateAccessor;

            if (templateAccessor.IsPresent()) {
                TemplateAccessor templateAccessor2 = entity.TemplateAccessor.Get();

                if (templateAccessor2 != null) {
                    TemplateDescription templateDescription = templateAccessor2.TemplateDescription;
                    entityBuilder.SetTemplate(templateDescription.TemplateClass);

                    if (templateAccessor2.HasConfigPath()) {
                        entityBuilder.SetConfig(templateAccessor2.ConfigPath);
                    } else {
                        YamlNode yamlNode = templateAccessor2.YamlNode;

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