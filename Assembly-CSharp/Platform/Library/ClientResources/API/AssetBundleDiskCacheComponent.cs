using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.Impl;

namespace Platform.Library.ClientResources.API {
    public class AssetBundleDiskCacheComponent : Component {
        public AssetBundleDiskCache AssetBundleDiskCache { get; set; }
    }
}