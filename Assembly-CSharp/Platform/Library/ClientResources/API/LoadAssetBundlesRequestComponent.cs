using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class LoadAssetBundlesRequestComponent : Component {
        public int LoadingPriority { get; set; }
    }
}