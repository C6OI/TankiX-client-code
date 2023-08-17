using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleStorageSystem : ECSSystem {
        [OnEventFire]
        public void Store(LoadCompleteEvent e, LoaderNode node) {
            string bundleName = node.assetBundleLoading.Info.BundleName;

            if (!AssetBundlesStorage.IsStored(bundleName)) {
                AssetBundlesStorage.Store(bundleName,
                    node.assetBundleLoading.Info,
                    node.assetBundleLoading.AssetBundleDiskCacheRequest.AssetBundle);
            }
        }

        [Mandatory]
        [OnEventFire]
        public void RefreshDependencies(AssetBundlesLoadedEvent e, SingleNode<AssetBundlesLoadDataComponent> loadDataNode) {
            foreach (AssetBundleInfo allBundle in loadDataNode.component.AllBundles) {
                AssetBundlesStorage.Refresh(allBundle.BundleName);
            }
        }

        bool IsBundleInLoadingDependencies(AssetBundleInfo info, ICollection<AssetBundleLoadingNode> loadingBundleNodes) =>
            loadingBundleNodes.Any(node => node.assetBundlesLoadData.AllBundles.Contains(info));

        public class LoaderNode : Node {
            public AssetBundleLoadingComponent assetBundleLoading;
        }

        public class AssetBundleLoadingNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }
    }
}