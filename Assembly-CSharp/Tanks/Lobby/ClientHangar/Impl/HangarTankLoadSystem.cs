using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientHangar.Impl.Builder;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankLoadSystem : HangarTankBaseSystem {
        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, WeaponItemPreviewNode weapon, [Context] [JoinByParentGroup] NotLoadedSkinPreviewItemNode skinItem,
            SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete) {
            skinItem.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, TankItemPreviewNode tank, [Context] [JoinByParentGroup] NotLoadedSkinPreviewItemNode skinItem,
            SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete) {
            skinItem.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedTankColoringPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete) {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedWeaponColoringPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete) {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedGraffityPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete) {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedContainerPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete) {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CancelItemAssetRequest(NodeRemoveEvent e, WeaponItemPreviewNode weapon, [JoinByParentGroup] LoadingSkinPreviewItemNode skin) {
            skin.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CancelItemAssetRequest(NodeRemoveEvent e, TankItemPreviewNode parent, [JoinByParentGroup] LoadingSkinPreviewItemNode item) {
            item.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, WeaponItemPreviewNode tank,
            [Context] [JoinByParentGroup] LoadingSkinPreviewItemNode tankSkin, [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, TankItemPreviewNode tank,
            [Context] [JoinByParentGroup] LoadingSkinPreviewItemNode tankSkin, [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, LoadingTankColoringPreviewNode tankColoring, [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, LoadingWeaponColoringPreviewNode tankColoring, [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, [Combine] LoadingGraffitiHangarPreviewItemNode graffity,
            [JoinAll] GraffitiLoadGearNode loadGear) {
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, [Combine] LoadingContainerPreviewNode container, [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeAddedEvent e, WeaponItemPreviewNode weapon, [Context] [JoinByParentGroup] LoadedSkinPreviewItemNode weaponSkin, TankItemPreviewNode tank,
            [Context] [JoinByParentGroup] LoadedSkinPreviewItemNode tankSkin, LoadedTankColoringPreviewItemNode tankColoring, LoadedWeaponColoringPreviewItemNode weaponColoring,
            [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeAddedEvent e, LoadedGraffitiHangarPreviewItemNode graffiti, [JoinAll] GraffitiLoadGearNode loadGear) {
            ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(HangarGraffitiBuildedEvent e, Node node, [JoinAll] GraffitiLoadGearNode loadGear) {
            ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeAddedEvent e, LoadedContainerPreviewItemNode container, LoadedTankColoringPreviewItemNode tankColoring,
            LoadedWeaponColoringPreviewItemNode weaponColoring, WeaponItemPreviewNode weapon, [JoinByParentGroup] [Context] LoadedSkinPreviewItemNode weaponSkin, TankItemPreviewNode tank,
            [Context] [JoinByParentGroup] LoadedSkinPreviewItemNode tankSkin, [JoinAll] TankLoadGearNode loadGear) {
            ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeRemoveEvent e, SingleNode<HangarInstanceComponent> hangar, [Combine] [JoinAll] SingleNode<LoadGearComponent> loadGear) {
            ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void UpdateGearProgress(UpdateEvent evt, ActiveTankLoadGearNode gear, [JoinAll] ICollection<LoadingNotGraffitiHangarPreviewItemStatsNode> items) {
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
        public void UpdateGearProgress(UpdateEvent evt, ActiveGraffitiLoadGearNode gear, [JoinAll] ICollection<LoadingGraffitiHangarPreviewItemStatsNode> items) {
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

        [Not(typeof(ResourceDataComponent))]
        public class LoadingHangarPreviewItemNode : HangarPreviewItemNode {
            public AssetReferenceComponent assetReference;
            public AssetRequestComponent assetRequest;
        }

        public class LoadedHangarPreviewItemNode : HangarPreviewItemNode {
            public ResourceDataComponent resourceData;
        }

        [Not(typeof(ResourceDataComponent))]
        [Not(typeof(AssetRequestComponent))]
        public class NotLoadedSkinPreviewItemNode : HangarPreviewItemNode {
            public AssetReferenceComponent assetReference;

            public SkinItemComponent skinItem;
        }

        public class LoadingSkinPreviewItemNode : LoadingHangarPreviewItemNode {
            public SkinItemComponent skinItem;
        }

        public class LoadedSkinPreviewItemNode : LoadedHangarPreviewItemNode {
            public ParentGroupComponent parentGroup;
            public SkinItemComponent skinItem;
        }

        [Not(typeof(ResourceDataComponent))]
        [Not(typeof(AssetRequestComponent))]
        public class NotLoadedTankColoringPreviewNode : HangarPreviewItemNode {
            public TankPaintItemComponent tankPaintItem;
        }

        public class LoadingTankColoringPreviewNode : LoadingHangarPreviewItemNode {
            public TankPaintItemComponent tankPaintItem;
        }

        public class LoadedTankColoringPreviewItemNode : LoadedHangarPreviewItemNode {
            public TankPaintItemComponent tankPaintItem;
        }

        [Not(typeof(ResourceDataComponent))]
        [Not(typeof(AssetRequestComponent))]
        public class NotLoadedWeaponColoringPreviewNode : HangarPreviewItemNode {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        public class LoadingWeaponColoringPreviewNode : LoadingHangarPreviewItemNode {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        public class LoadedWeaponColoringPreviewItemNode : LoadedHangarPreviewItemNode {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        [Not(typeof(ResourceDataComponent))]
        [Not(typeof(AssetRequestComponent))]
        public class NotLoadedGraffityPreviewNode : HangarPreviewItemNode {
            public GraffitiItemComponent graffitiItem;
        }

        public class LoadingGraffitiHangarPreviewItemNode : LoadingHangarPreviewItemNode {
            public GraffitiItemComponent graffitiItem;
        }

        public class LoadedGraffitiHangarPreviewItemNode : LoadedHangarPreviewItemNode {
            public GraffitiItemComponent graffitiItem;
        }

        [Not(typeof(ResourceDataComponent))]
        [Not(typeof(AssetRequestComponent))]
        public class NotLoadedContainerPreviewNode : HangarPreviewItemNode {
            public ContainerMarkerComponent containerMarker;
        }

        public class LoadingContainerPreviewNode : LoadingHangarPreviewItemNode {
            public ContainerMarkerComponent containerMarker;
        }

        public class LoadedContainerPreviewItemNode : LoadedHangarPreviewItemNode {
            public ContainerMarkerComponent containerMarker;
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