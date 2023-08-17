using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class SharedChangeableComponent : Component, ComponentLifecycle {
        EntityInternal entity;

        public void AttachToEntity(Entity entity) => this.entity = (EntityInternal)entity;

        public void DetachFromEntity(Entity entity) => entity = null;

        public void OnChange() {
            if (entity != null && entity.HasComponent(GetType())) {
                entity.NotifyComponentChange(GetType());
            }
        }
    }
}