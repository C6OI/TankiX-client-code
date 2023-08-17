using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.Impl;

namespace Platform.Library.ClientResources.API {
    public class ResourceLoadStatComponent : Component {
        public ResourceLoadStatComponent() {
            BundleToProgress = new Dictionary<AssetBundleInfo, float>();
            BytesLoaded = 0;
            Progress = 0f;
        }

        public Dictionary<AssetBundleInfo, float> BundleToProgress { get; private set; }

        public int BytesTotal { get; set; }

        public int BytesLoaded { get; set; }

        public float Progress { get; set; }
    }
}