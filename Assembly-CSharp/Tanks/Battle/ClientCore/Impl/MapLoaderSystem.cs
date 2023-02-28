using System.Collections.Generic;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Battle.ClientCore.Impl {
    public class MapLoaderSystem : ECSSystem {
        [OnEventComplete]
        public void LoadMapResources(NodeAddedEvent e, BattleUserNode user, [JoinByBattle] [Context] BattleNode battle, [JoinByMap] [Context] MapNode map,
            [JoinAll] ICollection<SingleNode<MapComponent>> maps) {
            maps.ForEach(delegate(SingleNode<MapComponent> m) {
                if (!m.Entity.Equals(map.Entity)) {
                    ScheduleEvent<CleanMapEvent>(m);
                }
            });

            map.Entity.AddComponent(new AssetRequestComponent(-100));
        }

        [OnEventFire]
        public void LoadMapScene(NodeAddedEvent e, LoadedMapNode map, [Context] BattleLoadScreenNode screen) {
            MarkAllObjectsAsUnloadedExceptHangar();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(map.resourceData.Name);
            ScheduleEvent(new LoadSceneEvent(fileNameWithoutExtension, map.resourceData.Data), map);
        }

        [OnEventFire]
        public void InitMap(NodeAddedEvent e, SingleNode<MapSceneLoadedMarkerComponent> mapSceneLoadedMarker, [JoinAll] [Context] LoadedMapNode map) {
            GameObject gameObject = mapSceneLoadedMarker.component.transform.parent.gameObject;

            if (!gameObject) {
                throw new CannotFindMapRootException(map.resourceData.Name);
            }

            EntityBehaviour entityBehaviour = gameObject.AddComponent<EntityBehaviour>();
            entityBehaviour.handleAutomaticaly = false;
            entityBehaviour.BuildEntity(map.Entity);
            map.Entity.AddComponent(new MapInstanceComponent(gameObject));
        }

        [OnEventComplete]
        public void InitMap(NodeAddedEvent e, SingleNode<MapSceneLoadedMarkerComponent> mapSceneLoadedMarker) {
            ScheduleEvent<UnloadUnusedAssetsEvent>(mapSceneLoadedMarker);
        }

        [OnEventFire]
        [Mandatory]
        public void PrepareToMapSceneUnloading(NodeRemoveEvent e, MapSceneNode map) {
            ScheduleEvent<CleanMapEvent>(map);
        }

        [OnEventFire]
        public void CleanMaps(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll] ICollection<SingleNode<MapComponent>> maps) {
            maps.ForEach(ScheduleEvent<CleanMapEvent>);
        }

        [OnEventFire]
        public void CleanMap(CleanMapEvent e, SingleNode<MapInstanceComponent> map) {
            map.Entity.RemoveComponent<MapInstanceComponent>();
        }

        [OnEventFire]
        public void CleanMap(CleanMapEvent e, SingleNode<AssetRequestComponent> map) {
            map.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CleanMap(CleanMapEvent e, SingleNode<ResourceDataComponent> map) {
            map.Entity.RemoveComponent<ResourceDataComponent>();
        }

        void MarkAllObjectsAsUnloadedExceptHangar() {
            int sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++) {
                Scene sceneAt = SceneManager.GetSceneAt(i);

                if (sceneAt.isLoaded && !SceneNames.HANGAR.Equals(sceneAt.name)) {
                    GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
                    GameObject[] array = rootGameObjects;

                    foreach (GameObject target in array) {
                        Object.DontDestroyOnLoad(target);
                    }
                }
            }
        }

        public class MapNode : Node {
            public AssetReferenceComponent assetReference;
            public MapComponent map;

            public MapGroupComponent mapGroup;
        }

        public class LoadedMapNode : Node {
            public MapComponent map;

            public ResourceDataComponent resourceData;
        }

        public class MapSceneNode : Node {
            public CurrentSceneComponent currentScene;
            public MapComponent map;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfBattleUserComponent selfBattleUser;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
            public MapGroupComponent mapGroup;
        }

        public class BattleLoadScreenNode : Node {
            public BattleLoadScreenComponent battleLoadScreen;

            public BattleLoadScreenReadyToHideComponent battleLoadScreenReadyToHide;
        }

        public class MapNodeWithRequest : Node {
            public AssetRequestComponent assetRequest;
            public MapComponent map;
        }
    }
}