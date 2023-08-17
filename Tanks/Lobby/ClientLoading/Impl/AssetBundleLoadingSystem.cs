using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientLoading.API;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class AssetBundleLoadingSystem : ECSSystem {
        [OnEventFire]
        public void DecreaseAssetBundleLoadingChannelsCount(NodeAddedEvent e, LoadCompletedNode loadCompleted,
            SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<AssetBundleLoadingChannelsCountComponent> assetBundleLoadingChannelsCount) =>
            assetBundleLoadingChannelsCount.component.ChannelsCount = 1;

        [OnEventFire]
        public void IncreaseAssetBundleLoadingChannelsCount(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<AssetBundleLoadingChannelsCountComponent> assetBundleLoadingChannelsCount) =>
            assetBundleLoadingChannelsCount.component.ChannelsCount =
                assetBundleLoadingChannelsCount.component.DefaultChannelsCount;

        public class LoadCompletedNode : Node {
            public BattleLoadScreenComponent battleLoadScreen;
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;
        }
    }
}