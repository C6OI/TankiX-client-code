using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class AssetsFirstLoadingSystem : ECSSystem {
        const string CONFIG_PATH = "clientlocal/clientresources/mandatoryassets";

        [Inject] public new static ConfigurationService ConfigurationService { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserNode selfUser) {
            Entity entity = CreateEntity("MandatoryAssetsFirstLoading");
            entity.AddComponent<MandatoryAssetsFirstLoadingComponent>();
            List<string> mandatoryAssetGUIDsFromConfig = GetMandatoryAssetGUIDsFromConfig(selfUser.Entity.Id);

            foreach (string item in mandatoryAssetGUIDsFromConfig) {
                AssetReferenceComponent component = new(new AssetReference(item));
                Entity entity2 = CreateEntity("Loader " + item);
                entity2.AddComponent(component);
                entity2.AddComponent<PreloadComponent>();
                entity2.AddComponent(new AssetRequestComponent(100));
            }
        }

        [OnEventFire]
        public void RequestHangarAssets(NodeAddedEvent e, SingleNode<SelfUserComponent> user, SingleNode<HangarAssetComponent> hangar,
            NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            hangar.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(hangar.component.AssetGuid)));
            RequestAsset(hangar.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.HANGAR, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestWeaponSkinAssets(NodeAddedEvent e, MountedWeaponSkinNode weaponSkin, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            RequestAsset(weaponSkin.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.WEAPON_SKIN, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestHullSkinAssets(NodeAddedEvent e, MountedHullSkinNode hullSkin, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            RequestAsset(hullSkin.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.HULL_SKIN, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestColoringAssets(NodeAddedEvent e, MountedTankPaintNode coloring, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            RequestAsset(coloring.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.TANK_COLORING, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestColoringAssets(NodeAddedEvent e, MountedWeaponPaintNode coloring, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            RequestAsset(coloring.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.WEAPON_COLORING, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestContainerAsset(NodeAddedEvent e, [Combine] ContainerNode container, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            if (!mandatoryLoading.mandatoryAssetsFirstLoading.IsContainerRequested()) {
                RequestAsset(container.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.CONTAINER, mandatoryLoading);
            }
        }

        [OnEventFire]
        public void CheckAssetRequestIsValid(NodeAddedEvent e, [Combine] AssetRequestNode node, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            if (!mandatoryLoading.mandatoryAssetsFirstLoading.IsAssetRequestMandatory(node.assetRequest)) {
                Log.InfoFormat("MandatoryAssetsLoading: Unexpected asset {0} will be loaded as mandatory!", node.assetReference.Reference.AssetGuid);
            }
        }

        [OnEventFire]
        public void OnPreloadingComplete(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> lobbyLoadScreen, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<ClientEnterLobbyEvent>(user);
        }

        [OnEventFire]
        public void StartPreload(NodeAddedEvent e, [Combine] SingleNode<LoadProgressTaskCompleteComponent> loadTaskNode, SingleNode<UserReadyForLobbyComponent> user,
            [JoinAll] ICollection<SingleNode<LobbyLoadScreenComponent>> screen) {
            if (screen.Count > 0 && !user.Entity.HasComponent<PreloadAllResourcesComponent>()) {
                user.Entity.AddComponent<PreloadAllResourcesComponent>();
            }
        }

        [OnEventFire]
        public void ShowPreloadAllResourcesLoadScreen(NodeAddedEvent e, SingleNode<PreloadAllResourcesComponent> node) {
            NewEvent<ShowPreloadScreenDelayedEvent>().Attach(node).ScheduleDelayed(0.5f);
        }

        [OnEventFire]
        public void ShowPreloadAllResourcesLoadScreen(ShowPreloadScreenDelayedEvent e, SingleNode<PreloadAllResourcesComponent> node,
            [JoinAll] ICollection<SingleNode<PreloadComponent>> preloads) {
            if (preloads.Count > 0) {
                ScheduleEvent<ShowScreenNoAnimationEvent<PreloadAllResourcesScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void OnComplete(ClientEnterLobbyEvent e, Node node, [JoinAll] NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            mandatoryLoading.Entity.AddComponent<MandatoryAssetsFirstLoadingCompletedComponent>();
        }

        void RequestAsset(Entity entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState requestsState, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            AssetRequestComponent assetRequestComponent = new(100);
            mandatoryLoading.mandatoryAssetsFirstLoading.MarkAsRequested(assetRequestComponent, requestsState);
            entity.AddComponent(assetRequestComponent);
            ShowLoadingScreenIfAllAssetsAreRequired(mandatoryLoading);
        }

        void ShowLoadingScreenIfAllAssetsAreRequired(NotCompletedMandatoryAssetsLoadingNode mandatoryLoading) {
            if (mandatoryLoading.mandatoryAssetsFirstLoading.AllMandatoryAssetsAreRequested()) {
                mandatoryLoading.mandatoryAssetsFirstLoading.LoadingScreenShown = true;
                ScheduleEvent<ShowScreenNoAnimationEvent<LobbyLoadScreenComponent>>(mandatoryLoading);
            }
        }

        static List<string> GetMandatoryAssetGUIDsFromConfig(long userId) {
            YamlNode config = ConfigurationService.GetConfig("clientlocal/clientresources/mandatoryassets");
            ConfigPathCollectionComponent configPathCollectionComponent = config.GetChildNode("configPathCollection").ConvertTo<ConfigPathCollectionComponent>();
            long num = userId % 3;
            num = 1L;

            if (num >= 1 && num <= 4) {
                switch (num - 1) {
                    case 0L:
                        return configPathCollectionComponent.Collection1;

                    case 1L:
                        return configPathCollectionComponent.Collection2;

                    case 2L:
                        return configPathCollectionComponent.Collection3;

                    case 3L:
                        return configPathCollectionComponent.Collection4;
                }
            }

            return configPathCollectionComponent.Collection;
        }

        public class MountedWeaponSkinNode : Node {
            public AssetReferenceComponent assetReference;

            public MountedItemComponent mountedItem;
            public UserItemComponent userItem;

            public WeaponSkinItemComponent weaponSkinItem;
        }

        public class MountedHullSkinNode : Node {
            public AssetReferenceComponent assetReference;

            public HullSkinItemComponent hullSkinItem;

            public MountedItemComponent mountedItem;
            public UserItemComponent userItem;
        }

        public class MountedTankPaintNode : Node {
            public AssetReferenceComponent assetReference;

            public MountedItemComponent mountedItem;

            public TankPaintItemComponent tankPaintItem;
            public UserItemComponent userItem;
        }

        public class MountedWeaponPaintNode : Node {
            public AssetReferenceComponent assetReference;

            public MountedItemComponent mountedItem;
            public UserItemComponent userItem;

            public WeaponPaintItemComponent weaponPaintItem;
        }

        [Not(typeof(MandatoryAssetsFirstLoadingCompletedComponent))]
        public class NotCompletedMandatoryAssetsLoadingNode : Node {
            public MandatoryAssetsFirstLoadingComponent mandatoryAssetsFirstLoading;
        }

        public class AssetRequestNode : Node {
            public AssetReferenceComponent assetReference;

            public AssetRequestComponent assetRequest;
        }

        public class ContainerNode : Node {
            public AssetReferenceComponent assetReference;
            public ContainerMarkerComponent containerMarker;

            public MarketItemComponent marketItem;
        }

        public class LobbyLoadCompletedNode : Node {
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;

            public LobbyLoadScreenComponent lobbyLoadScreen;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        public class ShowPreloadScreenDelayedEvent : Event { }
    }
}