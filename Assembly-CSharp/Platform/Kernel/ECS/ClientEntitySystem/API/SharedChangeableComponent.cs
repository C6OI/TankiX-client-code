using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class SharedChangeableComponent : Component, AttachToEntityListener, DetachFromEntityListener {
        EntityInternal entity;

        public void AttachedToEntity(Entity entity) {
            this.entity = (EntityInternal)entity;
        }

        public void DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void OnChange() {
            if (entity != null && entity.HasComponent(GetType())) {
                entity.NotifyComponentChange(GetType());
            }
        }
    }
}