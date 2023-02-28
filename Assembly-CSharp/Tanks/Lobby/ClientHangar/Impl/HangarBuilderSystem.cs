using System.Collections.Generic;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientHangar.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarBuilderSystem : ECSSystem {
        [OnEventComplete]
        public void LoadHangarResourcesOnBattleExit(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<HangarAssetComponent> hangar,
            [JoinAll] ICollection<SingleNode<MapComponent>> maps) {
            maps.ForEach(delegate(SingleNode<MapComponent> m) {
                ScheduleEvent<CleanMapEvent>(m.Entity);
            });

            if (!hangar.Entity.HasComponent<AssetRequestComponent>()) {
                hangar.Entity.AddComponent(new AssetRequestComponent(-100));
            }
        }

        [OnEventFire]
        public void LoadHangarResourcesOnBattleExit(LoadHangarEvent e, Node any, [JoinAll] SingleNode<HangarAssetComponent> hangar, [JoinAll] ICollection<SingleNode<MapComponent>> maps) {
            maps.ForEach(delegate(SingleNode<MapComponent> m) {
                ScheduleEvent<CleanMapEvent>(m.Entity);
            });

            if (!hangar.Entity.HasComponent<AssetRequestComponent>()) {
                hangar.Entity.AddComponent(new AssetRequestComponent(-100));
            }
        }

        [OnEventFire]
        public void LoadHangarScene(NodeAddedEvent e, HangarResourceNode hangar, SingleNode<SoundListenerResourcesComponent> readySoundListener) {
            MarkAllGameObjectsAsUnloadedExceptMap();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(hangar.resourceData.Name);
            ScheduleEvent(new LoadSceneEvent(fileNameWithoutExtension, hangar.resourceData.Data), hangar);
        }

        [OnEventComplete]
        public void InitHangarScene(NodeAddedEvent e, SingleNode<HangarSceneLoadedMarkerComponent> hangarSceneLoadedMarker, [JoinAll] [Mandatory] HangarResourceNode hangar) {
            GameObject gameObject = hangarSceneLoadedMarker.component.transform.parent.gameObject;
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            component.BuildEntity(hangar.Entity);
            HangarLocationsComponent hangarLocationsComponent = new();
            hangarLocationsComponent.Locations = new Dictionary<HangarLocation, Transform>();
            HangarLocationBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<HangarLocationBehaviour>(true);
            HangarLocationBehaviour[] array = componentsInChildren;

            foreach (HangarLocationBehaviour hangarLocationBehaviour in array) {
                hangarLocationsComponent.Locations.Add(hangarLocationBehaviour.HangarLocation, hangarLocationBehaviour.transform);
            }

            hangar.Entity.AddComponent(hangarLocationsComponent);
            hangar.Entity.AddComponent(new HangarInstanceComponent(gameObject));
            Object.Destroy(hangarSceneLoadedMarker.component.gameObject);
        }

        [OnEventFire]
        public void UnloadUnusedResources(NodeAddedEvent e, SingleNode<HangarSceneLoadedMarkerComponent> hangarSceneLoadedMarker, [JoinAll] [Mandatory] HangarResourceNode hangar) {
            ScheduleEvent<UnloadUnusedAssetsEvent>(hangar);
        }

        [OnEventFire]
        public void CleanHangarOnBattle(NodeAddedEvent e, ReadyToBattleUser battleUser, [JoinAll] SingleNode<HangarAssetComponent> hangar) {
            ScheduleEvent<CleanHangarEvent>(hangar);
        }

        [OnEventFire]
        public void PrepareForHangarSceneUnloading(NodeRemoveEvent e, HangarSceneNode hangarScene) {
            ScheduleEvent<CleanHangarEvent>(hangarScene);
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<HangarInstanceComponent> hangarScene) {
            hangarScene.Entity.RemoveComponent<HangarInstanceComponent>();
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<AssetRequestComponent> hangarScene) {
            hangarScene.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<ResourceDataComponent> hangarScene) {
            hangarScene.Entity.RemoveComponent<ResourceDataComponent>();
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<HangarLocationsComponent> hangarScene) {
            hangarScene.Entity.RemoveComponent<HangarLocationsComponent>();
        }

        [OnEventFire]
        public void HideScreenForeground(NodeRemoveEvent e, InstantiatedHangarNode node, [JoinAll] SingleNode<ScreenForegroundComponent> screenForeground) {
            ScheduleEvent<ForceHideScreenForegroundEvent>(screenForeground);
        }

        void MarkAllGameObjectsAsUnloadedExceptMap() {
            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++) {
                Scene sceneAt = SceneManager.GetSceneAt(i);

                if (!sceneAt.isLoaded) {
                    continue;
                }

                GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();

                foreach (GameObject gameObject in rootGameObjects) {
                    if (!ImportantGameObjectsNames.MAP_ROOT.Equals(gameObject.name.ToLower())) {
                        Object.DontDestroyOnLoad(gameObject);
                    }
                }
            }
        }

        public class HangarResourceNode : Node {
            public HangarAssetComponent hangarAsset;

            public ResourceDataComponent resourceData;
        }

        public class InstantiatedHangarNode : Node {
            public HangarInstanceComponent hangarInstance;
            public ResourceDataComponent resourceData;
        }

        public class HangarSceneNode : Node {
            public CurrentSceneComponent currentScene;
            public HangarComponent hangar;
        }

        public class ReadyToBattleUser : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ReadyToLobbyUser : Node {
            public SelfUserComponent selfUser;

            public UserReadyForLobbyComponent userReadyForLobby;
        }
    }
}