using LeopotamGroup.Pooling;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PoolContainerSystem : ECSSystem {
        [OnEventFire]
        public void InitContainer(NodeAddedEvent e, MapInstance instance) {
            GameObject gameObject = new("MainPoolContainer");
            gameObject.transform.parent = instance.mapInstance.SceneRoot.transform;
            MainPoolContainerComponent mainPoolContainerComponent = new();
            mainPoolContainerComponent.MainContainerTransform = gameObject.transform;
            MainPoolContainerComponent component = mainPoolContainerComponent;
            instance.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void CleanContainer(NodeRemoveEvent e, MapInstance map) {
            if (map.Entity.HasComponent<MainPoolContainerComponent>()) {
                map.Entity.RemoveComponent<MainPoolContainerComponent>();
            }
        }

        [OnEventFire]
        public void GetInstanceFromContainer(GetInstanceFromPoolEvent e, Node any, [JoinAll] Optional<MapWithContainer> map) {
            if (!map.IsPresent()) {
                e.Instance = Object.Instantiate(e.Prefab).transform;

                if (e.AutoRecycleTime > 0f) {
                    Object.Destroy(e.Instance.gameObject, e.AutoRecycleTime);
                }

                return;
            }

            MainPoolContainerComponent mainPoolContainer = map.Get().mainPoolContainer;
            PoolContainer value;

            if (!mainPoolContainer.PrefabToPoolDict.TryGetValue(e.Prefab, out value)) {
                Transform transform = new GameObject(e.Prefab.name + "_PoolContainer").transform;
                transform.SetParent(mainPoolContainer.MainContainerTransform);
                PoolContainer poolContainer = transform.gameObject.AddComponent<PoolContainer>();
                poolContainer.ItemsRoot = transform;
                poolContainer.CustomPrefab = e.Prefab;
                value = poolContainer;
                mainPoolContainer.PrefabToPoolDict.Add(e.Prefab, poolContainer);
            }

            IPoolObject poolObject = value.Get();
            Transform transform2 = e.Instance = poolObject.PoolTransform;

            if (!(e.AutoRecycleTime <= 0f)) {
                RecycleAfterTime component = transform2.gameObject.GetComponent<RecycleAfterTime>();

                if ((bool)component) {
                    component.Timeout = e.AutoRecycleTime;
                } else {
                    transform2.gameObject.AddComponent<RecycleAfterTime>().Timeout = e.AutoRecycleTime;
                }
            }
        }

        public class MapInstance : Node {
            public MapInstanceComponent mapInstance;
        }

        public class MapWithContainer : MapInstance {
            public MainPoolContainerComponent mainPoolContainer;
        }
    }
}