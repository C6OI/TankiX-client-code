namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentServerChangeListener {
        void ChangedOnServer(Entity entity);
    }
}