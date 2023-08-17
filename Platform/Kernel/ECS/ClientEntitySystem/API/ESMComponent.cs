using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class ESMComponent : Component, ComponentLifecycle {
        public EntityStateMachine esm = new EntityStateMachineImpl();

        public EntityStateMachine Esm => esm;

        public void AttachToEntity(Entity entity) => Esm.AttachToEntity(entity);

        public void DetachFromEntity(Entity entity) { }
    }
}