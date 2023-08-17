using System.Collections.Generic;
using System.IO;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientHangar.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarBuilderSystem : ECSSystem {
        [OnEventFire]
        public void LoadHangarResources(NodeAddedEvent e, SingleNode<SelfUserComponent> user,
            SingleNode<HangarAssetComponent> hangar) {
            hangar.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(hangar.component.AssetGuid)));
            hangar.Entity.AddComponent(new AssetRequestComponent(-100));
        }

        [OnEventComplete]
        public void LoadHangarResourcesOnBattleExit(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser,
            [JoinAll] SingleNode<HangarAssetComponent> hangar) =>
            hangar.Entity.AddComponent(new AssetRequestComponent(-100));

        [OnEventFire]
        public void LoadHangarScene(NodeAddedEvent e, HangarResourceNode hangar,
            SingleNode<SoundListenerResourcesComponent> readySoundListener) {
            MarkAllGameObjectsAsUnloadedExceptMap();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(hangar.resourceData.Name);
            ScheduleEvent(new LoadSceneEvent(fileNameWithoutExtension, hangar.resourceData.Data), hangar);
        }

        [OnEventComplete]
        public void InitHangarScene(NodeAddedEvent e, SingleNode<HangarSceneLoadedMarkerComponent> hangarSceneLoadedMarker,
            [Mandatory] [JoinAll] HangarResourceNode hangar) {
            GameObject gameObject = hangarSceneLoadedMarker.component.transform.parent.gameObject;
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            component.BuildEntity(hangar.Entity);
            HangarLocationsComponent hangarLocationsComponent = new();
            hangarLocationsComponent.Locations = new Dictionary<HangarLocation, Transform>();

            HangarLocationBehaviour[] componentsInChildren =
                gameObject.GetComponentsInChildren<HangarLocationBehaviour>(true);

            HangarLocationBehaviour[] array = componentsInChildren;

            foreach (HangarLocationBehaviour hangarLocationBehaviour in array) {
                hangarLocationsComponent.Locations.Add(hangarLocationBehaviour.HangarLocation,
                    hangarLocationBehaviour.transform);
            }

            hangar.Entity.AddComponent(hangarLocationsComponent);
            hangar.Entity.AddComponent(new HangarInstanceComponent(gameObject));
            Object.Destroy(hangarSceneLoadedMarker.component.gameObject);
        }

        [OnEventFire]
        public void UnloadUnusedResources(NodeAddedEvent e,
            SingleNode<HangarSceneLoadedMarkerComponent> hangarSceneLoadedMarker,
            [Mandatory] [JoinAll] HangarResourceNode hangar) => ScheduleEvent<UnloadUnusedResourcesEvent>(hangar);

        [OnEventFire]
        public void PrepareForHangarSceneUnloading(NodeRemoveEvent e, HangarSceneNode hangarScene) {
            hangarScene.Entity.RemoveComponent<HangarInstanceComponent>();
            hangarScene.Entity.RemoveComponent<AssetRequestComponent>();
            hangarScene.Entity.RemoveComponent<ResourceDataComponent>();
            hangarScene.Entity.RemoveComponent<HangarLocationsComponent>();
        }

        [OnEventFire]
        public void HideScreenForeground(NodeRemoveEvent e, InstantiatedHangarNode node,
            [JoinAll] SingleNode<ScreenForegroundComponent> screenForeground) =>
            ScheduleEvent<ForceHideScreenForegroundEvent>(screenForeground);

        void MarkAllGameObjectsAsUnloadedExceptMap() {
            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++) {
                Scene sceneAt = SceneManager.GetSceneAt(i);

                if (!sceneAt.isLoaded) {
                    continue;
                }

                GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();

                foreach (GameObject gameObject in rootGameObjects) {
                    if (gameObject.name.ToLower() != "map") {
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
    }
}