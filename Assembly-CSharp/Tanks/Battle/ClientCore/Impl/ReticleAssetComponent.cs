using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ReticleAssetComponent : Component {
        public ReticleAssetComponent() { }

        public ReticleAssetComponent(AssetReference reference) => Reference = reference;

        public AssetReference Reference { get; set; }
    }
}