using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleLoadingComponent : Component {
        public AssetBundleLoadingComponent() { }

        public AssetBundleLoadingComponent(AssetBundleInfo info) => Info = info;

        public AssetBundleInfo Info { get; set; }

        public float StartTime { get; set; }

        public AssetBundleDiskCacheRequest AssetBundleDiskCacheRequest { get; set; }

        public float Progress { get; set; }
    }
}