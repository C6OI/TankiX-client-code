using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class ResourcesWarmupSystem : ECSSystem {
        const int RESOURCE_WARMUP_COUNT_PER_FRAME = 3;

        [OnEventFire]
        public void RequestWarmupResources(NodeAddedEvent e, WarmupResourcesNode node) {
            AssetRequestComponent assetRequestComponent = new();
            assetRequestComponent.ResourceStoreLevel = ResourceStoreLevel.STATIC;
            assetRequestComponent.Priority = 100;
            node.Entity.AddComponent(assetRequestComponent);

            List<AssetReference> references =
                node.warmupResources.AssetGuids.Select(guid => new AssetReference(guid)).ToList();

            node.Entity.AddComponent(new AssetReferenceListComponent(references));
            node.Entity.AddComponent<ResourceWarmupIndexComponent>();
        }

        [OnEventFire]
        public void WarmupResources(UpdateEvent e, LoadedWarmupResources node,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            List<Object> dataList = node.resourceDataList.DataList;
            int count = dataList.Count;
            int num = node.resourceWarmupIndex.Index;

            for (int i = 0; i < 3; i++) {
                if (num >= count) {
                    node.Entity.AddComponent<WarmupResourcesPreparedComponent>();
                    return;
                }

                GameObject gameObject = dataList[num] as GameObject;

                if (gameObject != null) {
                    GameObject gameObject2 = Object.Instantiate(gameObject);
                    WarmInstanceComponents(gameObject2);
                    Object.Destroy(gameObject2);
                }

                num++;
            }

            node.resourceWarmupIndex.Index = num;
        }

        void WarmInstanceComponents(GameObject instance) {
            WarmableResourceBehaviour[] componentsInChildren =
                instance.GetComponentsInChildren<WarmableResourceBehaviour>(true);

            int num = componentsInChildren.Length;

            for (int i = 0; i < num; i++) {
                componentsInChildren[i].WarmUp();
            }
        }

        public class WarmupResourcesNode : Node {
            public WarmupResourcesComponent warmupResources;
        }

        [Not(typeof(WarmupResourcesPreparedComponent))]
        public class LoadedWarmupResources : Node {
            public ResourceDataListComponent resourceDataList;

            public ResourceWarmupIndexComponent resourceWarmupIndex;
            public WarmupResourcesComponent warmupResources;
        }
    }
}