using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class AsyncLoadingAssetComponent : Component {
        public LoadAssetFromBundleRequest Request { get; set; }
    }
}