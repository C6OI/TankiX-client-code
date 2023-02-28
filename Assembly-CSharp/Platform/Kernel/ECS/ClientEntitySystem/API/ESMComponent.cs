using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class ESMComponent : Component, AttachToEntityListener {
        public EntityStateMachine esm = new EntityStateMachineImpl();

        public EntityStateMachine Esm => esm;

        public void AttachedToEntity(Entity entity) {
            Esm.AttachToEntity(entity);
        }
    }
}