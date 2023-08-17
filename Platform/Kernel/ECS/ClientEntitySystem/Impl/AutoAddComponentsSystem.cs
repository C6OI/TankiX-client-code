using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AutoAddComponentsSystem : ECSSystem {
        [Inject] public static GroupRegistry GroupRegistry { get; set; }

        [OnEventFire]
        public void AutoAddComponentsIfNeedOnLoadedEntity(NodeAddedEvent e, SingleNode<SharedEntityComponent> any) =>
            AutoAddComponentsIfNeed(any);

        [OnEventFire]
        public void AutoAddComponentsIfNeedOnNewEntity(NodeAddedEvent e, SingleNode<NewEntityComponent> any) =>
            AutoAddComponentsIfNeed(any);

        void AutoAddComponentsIfNeed(Node any) {
            EntityInternal entityInternal = (EntityInternal)any.Entity;

            if (entityInternal.TemplateAccessor.IsPresent()) {
                TemplateDescription templateDescription = entityInternal.TemplateAccessor.Get().TemplateDescription;
                AutoAddComponents(entityInternal, templateDescription);
            }

            ScheduleEvent<ComponentsAutoAddedEvent>(entityInternal);
        }

        void AutoAddComponents(EntityInternal newEntity, TemplateDescription templateDescription) {
            ICollection<Type> autoAddedComponentTypes = templateDescription.GetAutoAddedComponentTypes();

            foreach (Type item in autoAddedComponentTypes) {
                Component component = !typeof(GroupComponent).IsAssignableFrom(item)
                                          ? newEntity.CreateNewComponentInstance(item)
                                          : GroupRegistry.FindOrCreateGroup(item, newEntity.Id);

                newEntity.AddComponent(component);
            }
        }
    }
}