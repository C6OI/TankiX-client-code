using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class AsyncLoadingAssetListComponent : Component {
        public AsyncLoadingAssetListComponent(List<LoadAssetFromBundleRequest> assetListRequest) => AssetListRequest = assetListRequest;

        public List<LoadAssetFromBundleRequest> AssetListRequest { get; set; }
    }
}