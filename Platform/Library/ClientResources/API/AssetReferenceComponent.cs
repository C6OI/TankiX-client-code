using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class AssetReferenceComponent : Component {
        public AssetReferenceComponent() { }

        public AssetReferenceComponent(AssetReference reference) => Reference = reference;

        public AssetReference Reference { get; set; }
    }
}