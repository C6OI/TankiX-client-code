using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetStorageSystem : ECSSystem {
        public static int MANAGED_RESOURCE_EXPIRE_DURATION_SEC = 300;

        [OnEventFire]
        public void Add(NodeAddedEvent e, SingleNode<AssetBundleDatabaseComponent> databaseNode) =>
            databaseNode.Entity.AddComponent<AssetStorageComponent>();

        [OnEventFire]
        public void Store(NodeAddedEvent e, LoadedAssetNode assetNode, [JoinAll] DatabaseNode db) {
            AssetStorageComponent assetStorage = db.assetStorage;
            string assetGuid = assetNode.assetReference.Reference.AssetGuid;
            Object data = assetNode.resourceData.Data;

            if (data != null) {
                assetStorage.Put(assetGuid, data, assetNode.assetRequest.ResourceStoreLevel);
            }
        }

        [OnEventFire]
        public void Store(NodeAddedEvent e, LoadedAssetListNode assetList, [JoinAll] DatabaseNode db) {
            AssetStorageComponent assetStorage = db.assetStorage;
            int num = 0;

            foreach (AssetReference reference in assetList.assetReferenceList.References) {
                string assetGuid = reference.AssetGuid;
                Object @object = assetList.resourceDataList.DataList[num++];

                if (@object != null) {
                    assetStorage.Put(assetGuid, @object, assetList.assetRequest.ResourceStoreLevel);
                }
            }
        }

        [OnEventFire]
        public void CleanStorage(UnloadUnusedResourcesEvent e, Node any, [JoinAll] DatabaseNode db) {
            AssetStorageComponent assetStorage = db.assetStorage;
            List<string> list = new(10);

            foreach (KeyValuePair<string, ResourceStorageEntry> managedReferency in db.assetStorage.ManagedReferencies) {
                ResourceStorageEntry value = managedReferency.Value;

                if (IsExpired(value)) {
                    list.Add(managedReferency.Key);
                }
            }

            foreach (string item in list) {
                assetStorage.Remove(item, ResourceStoreLevel.MANAGED);
            }
        }

        bool IsExpired(ResourceStorageEntry entry) =>
            entry.LastAccessTime + MANAGED_RESOURCE_EXPIRE_DURATION_SEC > Time.time;

        public class DatabaseNode : Node {
            public AssetBundleDatabaseComponent assetBundleDatabase;

            public AssetStorageComponent assetStorage;
        }

        public class LoadedAssetNode : Node {
            public AssetReferenceComponent assetReference;
            public AssetRequestComponent assetRequest;

            public ResourceDataComponent resourceData;
        }

        public class LoadedAssetListNode : Node {
            public AssetReferenceListComponent assetReferenceList;
            public AssetRequestComponent assetRequest;

            public ResourceDataListComponent resourceDataList;
        }
    }
}