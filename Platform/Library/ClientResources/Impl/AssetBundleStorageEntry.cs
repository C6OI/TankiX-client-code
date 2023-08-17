using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleStorageEntry {
        public float LastAccessTime { get; set; }

        public AssetBundle Bundle { get; set; }

        public AssetBundleInfo Info { get; set; }
    }
}