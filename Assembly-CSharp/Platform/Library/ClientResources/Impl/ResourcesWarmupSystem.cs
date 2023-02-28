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
            assetRequestComponent.AssetStoreLevel = AssetStoreLevel.STATIC;
            assetRequestComponent.Priority = 100;
            node.Entity.AddComponent(assetRequestComponent);
            List<AssetReference> list = node.warmupResources.AssetGuids.Select(guid => new AssetReference(guid)).ToList();
            node.Entity.AddComponent<ResourceWarmupIndexComponent>();
        }

        void WarmInstanceComponents(GameObject instance) {
            WarmableResourceBehaviour[] componentsInChildren = instance.GetComponentsInChildren<WarmableResourceBehaviour>(true);
            int num = componentsInChildren.Length;

            for (int i = 0; i < num; i++) {
                componentsInChildren[i].WarmUp();
            }
        }

        public class WarmupResourcesNode : Node {
            public WarmupResourcesComponent warmupResources;
        }
    }
}