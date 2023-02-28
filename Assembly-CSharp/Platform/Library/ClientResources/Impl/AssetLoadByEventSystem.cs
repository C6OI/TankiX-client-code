using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class AssetLoadByEventSystem : ECSSystem {
        [OnEventFire]
        public void ProcessRequest(AssetRequestEvent e, Node any) {
            Entity entity = CreateEntity("AssetLoadByEventRequest");
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(e.AssetGuid)));
            LoadByEventRequestComponent loadByEventRequestComponent = new();
            loadByEventRequestComponent.ResourceDataComponentType = e.ResourceDataComponentType;
            loadByEventRequestComponent.Owner = any.Entity;
            entity.AddComponent(loadByEventRequestComponent);
            AssetRequestComponent assetRequestComponent = new();
            assetRequestComponent.Priority = e.Priority;
            assetRequestComponent.AssetStoreLevel = e.StoreLevel;
            entity.AddComponent(assetRequestComponent);
            e.LoaderEntity = entity;
        }

        [OnEventFire]
        public void Complete(NodeAddedEvent e, LoaderWithDataNode loaderWithData) {
            Type resourceDataComponentType = loaderWithData.loadByEventRequest.ResourceDataComponentType;

            if (resourceDataComponentType != null) {
                Entity owner = loaderWithData.loadByEventRequest.Owner;
                bool flag = owner.HasComponent(resourceDataComponentType);
                Log.InfoFormat("Complete {0} hasComponent={1}", resourceDataComponentType, flag);

                if (!flag) {
                    ResourceDataComponent resourceDataComponent = (ResourceDataComponent)owner.CreateNewComponentInstance(resourceDataComponentType);
                    resourceDataComponent.Data = loaderWithData.resourceData.Data;
                    owner.AddComponent(resourceDataComponent);
                }
            }

            if (loaderWithData.Entity.Alive) {
                DeleteEntity(loaderWithData.Entity);
            }
        }

        public class LoaderWithDataNode : Node {
            public LoadByEventRequestComponent loadByEventRequest;

            public ResourceDataComponent resourceData;
        }
    }
}