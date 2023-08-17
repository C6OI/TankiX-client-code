using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.Impl {
    public class ResourceGroupComponent : GroupComponent {
        public ResourceGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ResourceGroupComponent(long key)
            : base(key) { }
    }
}