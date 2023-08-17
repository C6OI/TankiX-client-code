using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class ResourceLoadHelperSystem : ECSSystem {
        [OnEventFire]
        public void ProcessRequest(ResourceRequestEvent e, Node any) {
            Entity entity = CreateEntity("ResourceLoad");
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(e.AssetGuid)));
            LoadHelperRequestComponent loadHelperRequestComponent = new();
            loadHelperRequestComponent.ResourceDataComponentType = e.ResourceDataComponentType;
            loadHelperRequestComponent.Owner = any.Entity;
            entity.AddComponent(loadHelperRequestComponent);
            AssetRequestComponent assetRequestComponent = new();
            assetRequestComponent.Priority = e.Priority;
            assetRequestComponent.ResourceStoreLevel = e.StoreLevel;
            entity.AddComponent(assetRequestComponent);
        }

        [OnEventFire]
        public void Complete(NodeAddedEvent e, LoaderWithDataNode loaderWithData) {
            Type resourceDataComponentType = loaderWithData.loadHelperRequest.ResourceDataComponentType;
            Entity owner = loaderWithData.loadHelperRequest.Owner;
            bool flag = owner.HasComponent(resourceDataComponentType);
            Log.InfoFormat("Complete {0} hasComponent={1}", resourceDataComponentType, flag);

            if (!flag) {
                ResourceDataComponent resourceDataComponent =
                    (ResourceDataComponent)owner.CreateNewComponentInstance(resourceDataComponentType);

                resourceDataComponent.Data = loaderWithData.resourceData.Data;
                owner.AddComponent(resourceDataComponent);
            }

            DeleteEntity(loaderWithData.Entity);
        }

        public class LoaderWithDataNode : Node {
            public LoadHelperRequestComponent loadHelperRequest;

            public ResourceDataComponent resourceData;
        }
    }
}