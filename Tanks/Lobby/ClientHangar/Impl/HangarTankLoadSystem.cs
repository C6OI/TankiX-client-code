using System.Collections.Generic;
using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientHangar.Impl.Builder;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankLoadSystem : HangarTankBaseSystem {
        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedHangarPreviewItemNode item) =>
            item.Entity.AddComponent<AssetRequestComponent>();

        [OnEventFire]
        public void CancelItemAssetRequest(NodeRemoveEvent e, LoadingHangarPreviewItemNRNode item,
            [JoinSelf] LoadingHangarPreviewItemNode node) => node.Entity.RemoveComponent<AssetRequestComponent>();

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar,
            [Combine] LoadingNotGraffitiHangarPreviewItemNode garageItem, [JoinAll] TankLoadGearNode loadGear) =>
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar,
            [Combine] LoadingGraffitiHangarPreviewItemNode garageItem, [JoinAll] GraffitiLoadGearNode loadGear) =>
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);

        [OnEventFire]
        public void HideLoadIndicator(HangarTankBuildedEvent e, HangarNode hangar, [JoinAll] TankLoadGearNode loadGear) =>
            ScheduleEvent<HideLoadGearEvent>(loadGear);

        [OnEventFire]
        public void HideLoadIndicator(HangarGraffitiBuildedEvent e, Node node, [JoinAll] GraffitiLoadGearNode loadGear) =>
            ScheduleEvent<HideLoadGearEvent>(loadGear);

        [OnEventFire]
        public void HideLoadIndicator(NodeRemoveEvent e, SingleNode<HangarInstanceComponent> hangar,
            [JoinAll] [Combine] SingleNode<LoadGearComponent> loadGear) => ScheduleEvent<HideLoadGearEvent>(loadGear);

        [OnEventFire]
        public void UpdateGearProgress(UpdateEvent evt, ActiveTankLoadGearNode gear,
            [JoinAll] ICollection<LoadingNotGraffitiHangarPreviewItemStatsNode> items) {
            int num = 0;
            int num2 = 0;

            foreach (LoadingNotGraffitiHangarPreviewItemStatsNode item in items) {
                ResourceLoadStatComponent resourceLoadStat = item.resourceLoadStat;
                num += resourceLoadStat.BytesTotal;
                num2 += resourceLoadStat.BytesLoaded;
            }

            float value = num <= 0 ? 1f : num2 / (float)num;
            ScheduleEvent(new UpdateLoadGearProgressEvent(value), gear);
        }

        [OnEventFire]
        public void UpdateGearProgress(UpdateEvent evt, ActiveGraffitiLoadGearNode gear,
            [JoinAll] ICollection<LoadingGraffitiHangarPreviewItemStatsNode> items) {
            int num = 0;
            int num2 = 0;

            foreach (LoadingGraffitiHangarPreviewItemStatsNode item in items) {
                ResourceLoadStatComponent resourceLoadStat = item.resourceLoadStat;
                num += resourceLoadStat.BytesTotal;
                num2 += resourceLoadStat.BytesLoaded;
            }

            float value = num <= 0 ? 1f : num2 / (float)num;
            ScheduleEvent(new UpdateLoadGearProgressEvent(value), gear);
        }

        [Not(typeof(AssetRequestComponent))]
        [Not(typeof(ResourceDataComponent))]
        public class NotLoadedHangarPreviewItemNode : HangarPreviewItemNode {
            public AssetReferenceComponent assetReference;
        }

        public class LoadedHangarPreviewItemNode : HangarPreviewItemNode {
            public ResourceDataComponent resourceData;
        }

        [Not(typeof(ResourceDataComponent))]
        public class LoadingHangarPreviewItemNode : HangarPreviewItemNode {
            public AssetReferenceComponent assetReference;
            public AssetRequestComponent assetRequest;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class LoadingNotGraffitiHangarPreviewItemNode : LoadingHangarPreviewItemNode { }

        public class LoadingGraffitiHangarPreviewItemNode : LoadingHangarPreviewItemNode {
            public GraffitiItemComponent graffitiItem;
        }

        [Not(typeof(ResourceDataComponent))]
        public class LoadingHangarPreviewItemNRNode : HangarPreviewItemNode {
            public AssetReferenceComponent assetReference;
        }

        [Not(typeof(ResourceDataComponent))]
        public class LoadingHangarPreviewItemStatsNode : LoadingHangarPreviewItemNode {
            public ResourceLoadStatComponent resourceLoadStat;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class LoadingNotGraffitiHangarPreviewItemStatsNode : LoadingHangarPreviewItemStatsNode { }

        public class LoadingGraffitiHangarPreviewItemStatsNode : LoadingHangarPreviewItemStatsNode {
            public GraffitiItemComponent graffitiItem;
        }

        public class TankLoadGearNode : Node {
            public LoadGearComponent loadGear;
            public ScreenForegroundComponent screenForeground;
        }

        public class GraffitiLoadGearNode : Node {
            public GraffitiGarageItemsScreenComponent graffitiGarageItemsScreen;

            public LoadGearComponent loadGear;
        }

        public class ActiveTankLoadGearNode : TankLoadGearNode {
            public ActiveGearComponent activeGear;
        }

        public class ActiveGraffitiLoadGearNode : GraffitiLoadGearNode {
            public ActiveGearComponent activeGear;
        }
    }
}