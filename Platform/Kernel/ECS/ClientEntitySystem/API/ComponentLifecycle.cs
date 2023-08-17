namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentLifecycle {
        void AttachToEntity(Entity entity);

        void DetachFromEntity(Entity entity);
    }
}