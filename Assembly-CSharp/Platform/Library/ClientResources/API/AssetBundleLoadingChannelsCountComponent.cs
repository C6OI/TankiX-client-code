using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class AssetBundleLoadingChannelsCountComponent : Component {
        public int ChannelsCount { get; set; }

        public int DefaultChannelsCount { get; set; }
    }
}