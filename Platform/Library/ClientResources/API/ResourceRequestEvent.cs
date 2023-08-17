using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class ResourceRequestEvent : Event {
        public string AssetGuid { get; set; }

        public Type ResourceDataComponentType { get; set; }

        public int Priority { get; set; }

        public ResourceStoreLevel StoreLevel { get; set; }

        public ResourceRequestEvent Init<T>(string assetGuid) where T : ResourceDataComponent {
            AssetGuid = assetGuid;
            ResourceDataComponentType = typeof(T);
            StoreLevel = ResourceStoreLevel.MANAGED;
            return this;
        }

        public ResourceRequestEvent Init<T>(string assetGuid, int priority, ResourceStoreLevel level)
            where T : ResourceDataComponent {
            AssetGuid = assetGuid;
            ResourceDataComponentType = typeof(T);
            Priority = priority;
            StoreLevel = level;
            return this;
        }
    }
}