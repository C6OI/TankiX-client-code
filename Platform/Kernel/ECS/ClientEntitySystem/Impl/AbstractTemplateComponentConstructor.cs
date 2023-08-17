using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public abstract class AbstractTemplateComponentConstructor : ComponentConstructor {
        public bool IsAcceptable(Type componentType, EntityInternal entity) {
            Optional<TemplateAccessor> templateAccessor = entity.TemplateAccessor;

            if (!templateAccessor.IsPresent()) {
                return false;
            }

            TemplateDescription templateDescription = templateAccessor.Get().TemplateDescription;

            if (!templateDescription.IsComponentDescriptionPresent(componentType)) {
                return false;
            }

            ComponentDescription componentDescription = templateDescription.GetComponentDescription(componentType);
            return IsAcceptable(componentDescription, entity);
        }

        public Component GetComponentInstance(Type componentType, EntityInternal entity) {
            TemplateDescription templateDescription = entity.TemplateAccessor.Get().TemplateDescription;
            ComponentDescription componentDescription = templateDescription.GetComponentDescription(componentType);
            return GetComponentInstance(componentDescription, entity);
        }

        protected abstract bool IsAcceptable(ComponentDescription componentDescription, EntityInternal entity);

        protected internal abstract Component GetComponentInstance(ComponentDescription componentDescription,
            EntityInternal entity);
    }
}