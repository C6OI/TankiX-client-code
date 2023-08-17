using Platform.Library.ClientResources.Impl;
using UnityEngine;

namespace Platform.Library.ClientResources.API {
    public class AssetBundleDiskCacheRequest {
        public AssetBundleDiskCacheRequest(AssetBundleInfo assetBundleInfo, bool createAssetBundle = true) {
            State = AssetBundleDiskCacheState.INIT;
            AssetBundleInfo = assetBundleInfo;
            CreateAssetBundle = createAssetBundle;
        }

        public bool UseCrcCheck { get; set; }

        public bool CreateAssetBundle { get; set; }

        public AssetBundleInfo AssetBundleInfo { get; set; }

        public bool IsDone { get; set; }

        public AssetBundle AssetBundle { get; set; }

        public string Error { get; set; }

        public float Progress { get; set; }

        public AssetBundleDiskCacheState State { get; set; }
    }
}