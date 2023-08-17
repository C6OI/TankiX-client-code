using System.IO;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Battle.ClientCore.Impl {
    public class MapLoaderSystem : ECSSystem {
        [OnEventFire]
        [Mandatory]
        public void LoadMapResources(NodeAddedEvent e, BattleUserNode user, [JoinByBattle] BattleNode battle,
            [JoinByMap] MapNode map) => map.Entity.AddComponent(new AssetRequestComponent(-100));

        [OnEventFire]
        [Mandatory]
        public void LoadMapScene(NodeAddedEvent e, LoadedMapNode map) {
            MarkAllObjectsAsUnloadedExceptHangar();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(map.resourceData.Name);
            ScheduleEvent(new LoadSceneEvent(fileNameWithoutExtension, map.resourceData.Data), map);
        }

        [OnEventFire]
        [Mandatory]
        public void InitMap(NodeAddedEvent e, SingleNode<MapSceneLoadedMarkerComponent> mapSceneLoadedMarker,
            [JoinAll] LoadedMapNode map) {
            GameObject gameObject = mapSceneLoadedMarker.component.transform.parent.gameObject;

            if (!gameObject) {
                throw new CannotFindMapRootException(map.resourceData.Name);
            }

            map.Entity.AddComponent(new MapInstanceComponent(gameObject));
            EntityBehaviour entityBehaviour = gameObject.AddComponent<EntityBehaviour>();
            entityBehaviour.handleAutomaticaly = false;
            entityBehaviour.BuildEntity(map.Entity);
        }

        [OnEventComplete]
        public void InitMap(NodeAddedEvent e, SingleNode<MapSceneLoadedMarkerComponent> mapSceneLoadedMarker) {
            ScheduleEvent<UnloadUnusedResourcesEvent>(mapSceneLoadedMarker);
            Object.Destroy(mapSceneLoadedMarker.component.gameObject);
        }

        [OnEventFire]
        [Mandatory]
        public void PrepareToMapSceneUnloading(NodeRemoveEvent e, MapSceneNode map) {
            map.Entity.RemoveComponent<AssetRequestComponent>();
            map.Entity.RemoveComponent<ResourceDataComponent>();
            map.Entity.RemoveComponent<MapInstanceComponent>();
        }

        void MarkAllObjectsAsUnloadedExceptHangar() {
            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++) {
                Scene sceneAt = SceneManager.GetSceneAt(i);

                if (sceneAt.isLoaded && sceneAt.name != SceneNames.HANGAR) {
                    GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();

                    for (int j = 0; j < rootGameObjects.Length; j++) {
                        Object.DontDestroyOnLoad(rootGameObjects[j]);
                    }
                }
            }
        }

        public class MapNode : Node {
            public AssetReferenceComponent assetReference;
            public MapComponent map;
        }

        public class LoadedMapNode : Node {
            public MapComponent map;

            public ResourceDataComponent resourceData;
        }

        public class MapSceneNode : Node {
            public CurrentSceneComponent currentScene;
            public MapComponent map;
        }

        public class InstantiatedMapNode : Node {
            public MapComponent map;

            public MapInstanceComponent mapInstance;

            public ResourceDataComponent resourceData;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfBattleUserComponent selfBattleUser;
        }

        public class BattleNode : Node {
            public BattleComponent battle;
            public MapGroupComponent mapGroup;
        }
    }
}