using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class AssetRequestComponent : Component {
        public AssetRequestComponent(int priority) => Priority = priority;

        public AssetRequestComponent() {
            Priority = 0;
            ResourceStoreLevel = ResourceStoreLevel.MANAGED;
        }

        public int Priority { get; set; }

        public ResourceStoreLevel ResourceStoreLevel { get; set; }
    }
}