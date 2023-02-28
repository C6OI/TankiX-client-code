using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.Impl {
    public class AssetGroupComponent : GroupComponent {
        public AssetGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public AssetGroupComponent(long key)
            : base(key) { }
    }
}