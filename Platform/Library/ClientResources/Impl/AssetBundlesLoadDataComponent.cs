using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundlesLoadDataComponent : Component {
        public bool Loaded { get; set; }

        public HashSet<AssetBundleInfo> AllBundles { get; set; }

        public HashSet<AssetBundleInfo> BundlesToLoad { get; set; }

        public HashSet<AssetBundleInfo> LoadingBundles { get; set; }

        public Dictionary<string, AssetBundle> LoadedBundles { get; set; }
    }
}