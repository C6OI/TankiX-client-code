namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentListener {
        void OnComponentAdded(Entity entity, Component component);

        void OnComponentRemoved(Entity entity, Component component);

        void OnComponentChanged(Entity entity, Component component);
    }
}