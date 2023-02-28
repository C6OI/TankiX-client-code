namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface DetachFromEntityListener {
        void DetachedFromEntity(Entity entity);
    }
}