using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class AssetRequestEvent : Event {
        public AssetRequestEvent() { }

        public AssetRequestEvent(AssetReference reference) => AssetGuid = reference.AssetGuid;

        public string AssetGuid { get; set; }

        public Type ResourceDataComponentType { get; set; }

        public int Priority { get; set; }

        public AssetStoreLevel StoreLevel { get; set; }

        public Entity LoaderEntity { get; set; }

        public AssetRequestEvent Init<T>(string assetGuid) where T : ResourceDataComponent {
            AssetGuid = assetGuid;
            ResourceDataComponentType = typeof(T);
            StoreLevel = AssetStoreLevel.MANAGED;
            return this;
        }

        public AssetRequestEvent Init<T>(string assetGuid, int priority, AssetStoreLevel level) where T : ResourceDataComponent {
            AssetGuid = assetGuid;
            ResourceDataComponentType = typeof(T);
            Priority = priority;
            StoreLevel = level;
            return this;
        }
    }
}