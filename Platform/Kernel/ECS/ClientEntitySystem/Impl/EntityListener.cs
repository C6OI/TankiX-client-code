using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface EntityListener {
        void OnNodeAdded(Entity entity, NodeDescription nodeDescription);

        void OnNodeRemoved(Entity entity, NodeDescription nodeDescription);

        void OnEntityDeleted(Entity entity);
    }
}