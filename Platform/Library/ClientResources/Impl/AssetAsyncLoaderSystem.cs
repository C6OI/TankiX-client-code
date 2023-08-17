using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetAsyncLoaderSystem : ECSSystem {
        [OnEventFire]
        public void ProcessAssetRequest(NodeAddedEvent e, [Combine] AssetRequestNode node, DatabaseNode db) {
            string assetGuid = node.assetReference.Reference.AssetGuid;
            AssetBundleDatabase assetBundleDatabase = db.assetBundleDatabase.AssetBundleDatabase;
            AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(assetGuid);
            Entity entity = node.Entity;

            if (db.assetStorage.Contains(assetGuid)) {
                Object data = db.assetStorage.Get(assetGuid);
                AttachAssetToEntity(data, assetInfo.ObjectName, entity);
                return;
            }

            LoadAssetBundlesRequestComponent loadAssetBundlesRequestComponent = new();
            loadAssetBundlesRequestComponent.LoadingPriority = node.assetRequest.Priority;
            LoadAssetBundlesRequestComponent component = loadAssetBundlesRequestComponent;
            node.Entity.AddComponent(component);
        }

        [OnEventComplete]
        public void StartLoadAssetFromBundle(AssetBundlesLoadedEvent e, AssetDependenciesNode asset,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            LoadAssetFromBundleRequest request =
                CreateLoadAssetRequest(asset.assetReference.Reference, db.component, asset.assetBundlesLoadData);

            AsyncLoadingAssetComponent asyncLoadingAssetComponent = new();
            asyncLoadingAssetComponent.Request = request;
            AsyncLoadingAssetComponent component = asyncLoadingAssetComponent;
            asset.Entity.AddComponent(component);
            asset.Entity.RemoveComponent<LoadAssetBundlesRequestComponent>();
        }

        [OnEventFire]
        public void CompleteLoadAssetFromBundle(UpdateEvent e, AsyncLoadingAssetNode loadingAsset,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            LoadAssetFromBundleRequest request = loadingAsset.asyncLoadingAsset.Request;

            if (request.IsDone) {
                Object asset = request.Asset;
                AssetReference reference = loadingAsset.assetReference.Reference;
                reference.SetReference(asset);
                loadingAsset.Entity.RemoveComponent<AsyncLoadingAssetComponent>();
                AttachAssetToEntity(asset, request.ObjectName, loadingAsset.Entity);
            }
        }

        [OnEventFire]
        public void ProcessAssetListRequest(NodeAddedEvent e, [Combine] AssetListRequestNode node, DatabaseNode db) {
            AssetStorageComponent assetStorage = db.assetStorage;
            IEnumerable<AssetReference> references = node.assetReferenceList.References;
            List<Object> assetList;

            if (FillAllAssetsFromStorage(references, assetStorage, out assetList)) {
                node.Entity.AddComponent(new ResourceDataListComponent(assetList));
                return;
            }

            LoadAssetBundlesRequestComponent loadAssetBundlesRequestComponent = new();
            loadAssetBundlesRequestComponent.LoadingPriority = node.assetRequest.Priority;
            LoadAssetBundlesRequestComponent component = loadAssetBundlesRequestComponent;
            node.Entity.AddComponent(component);
        }

        [OnEventComplete]
        public void StartLoadAssetListFromBundles(AssetBundlesLoadedEvent e, AssetListDependenciesNode assetList,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            List<AssetReference> references = assetList.assetReferenceList.References;
            List<LoadAssetFromBundleRequest> list = new();

            for (int i = 0; i < references.Count; i++) {
                LoadAssetFromBundleRequest item =
                    CreateLoadAssetRequest(references[i], db.component, assetList.assetBundlesLoadData);

                list.Add(item);
            }

            AsyncLoadingAssetListComponent component = new(list);
            assetList.Entity.AddComponent(component);
            assetList.Entity.RemoveComponent<LoadAssetBundlesRequestComponent>();
        }

        [OnEventFire]
        public void CompleteLoadAssetListFromBundle(UpdateEvent e, AsyncLoadingAssetListNode assetList,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            List<LoadAssetFromBundleRequest> assetListRequest = assetList.asyncLoadingAssetList.AssetListRequest;

            foreach (LoadAssetFromBundleRequest item in assetListRequest) {
                if (!item.IsDone) {
                    return;
                }
            }

            List<Object> list = new();
            List<AssetReference> references = assetList.assetReferenceList.References;

            for (int i = 0; i < references.Count; i++) {
                Object asset = assetListRequest[i].Asset;
                list.Add(asset);
                references[i].SetReference(asset);
            }

            assetList.Entity.RemoveComponent<AsyncLoadingAssetListComponent>();
            ResourceDataListComponent resourceDataListComponent = new();
            resourceDataListComponent.DataList = list;
            ResourceDataListComponent component = resourceDataListComponent;
            assetList.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void CancelAssetBundleLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest,
            [JoinSelf] SingleNode<LoadAssetBundlesRequestComponent> loadAssetBundlesRequest) =>
            assetRequest.Entity.RemoveComponent<LoadAssetBundlesRequestComponent>();

        [OnEventFire]
        public void CancelAssetLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest,
            [JoinSelf] SingleNode<AsyncLoadingAssetComponent> loadAssetBundlesRequest) =>
            assetRequest.Entity.RemoveComponent<AsyncLoadingAssetComponent>();

        [OnEventFire]
        public void CancelAssetListLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest,
            [JoinSelf] SingleNode<AsyncLoadingAssetListComponent> loadAssetBundlesRequest) =>
            assetRequest.Entity.RemoveComponent<AsyncLoadingAssetListComponent>();

        static AsyncLoadAssetFromBundleRequest CreateLoadAssetRequest(AssetReference assetReference,
            AssetBundleDatabaseComponent db, AssetBundlesLoadDataComponent assetBundlesLoadData) {
            AssetBundleDatabase assetBundleDatabase = db.AssetBundleDatabase;
            AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(assetReference.AssetGuid);
            AssetBundle bundle = assetBundlesLoadData.LoadedBundles[assetInfo.AssetBundleInfo.BundleName];
            return new AsyncLoadAssetFromBundleRequest(bundle, assetInfo.ObjectName, assetInfo.AssetType);
        }

        bool FillAllAssetsFromStorage(IEnumerable<AssetReference> referencies, AssetStorageComponent storage,
            out List<Object> assetList) {
            assetList = new List<Object>(5);

            foreach (AssetReference referency in referencies) {
                if (storage.Contains(referency.AssetGuid)) {
                    assetList.Add(storage.Get(referency.AssetGuid));
                    continue;
                }

                return false;
            }

            return true;
        }

        public void AttachAssetToEntity(Object data, string name, Entity entity) {
            ResourceDataComponent resourceDataComponent = new();
            resourceDataComponent.Data = data;
            resourceDataComponent.Name = name;
            entity.AddComponent(resourceDataComponent);
        }

        public class AssetRequestNode : Node {
            public AssetReferenceComponent assetReference;

            public AssetRequestComponent assetRequest;
        }

        public class AssetListRequestNode : Node {
            public AssetReferenceListComponent assetReferenceList;

            public AssetRequestComponent assetRequest;
        }

        public class AssetDependenciesNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetReferenceComponent assetReference;

            public AssetRequestComponent assetRequest;

            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class AssetListDependenciesNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetReferenceListComponent assetReferenceList;

            public AssetRequestComponent assetRequest;

            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class DatabaseNode : Node {
            public AssetBundleDatabaseComponent assetBundleDatabase;

            public AssetStorageComponent assetStorage;
        }

        public class AsyncLoadingAssetNode : Node {
            public AssetReferenceComponent assetReference;

            public AssetRequestComponent assetRequest;

            public AsyncLoadingAssetComponent asyncLoadingAsset;
        }

        public class AsyncLoadingAssetListNode : Node {
            public AssetReferenceListComponent assetReferenceList;

            public AssetRequestComponent assetRequest;

            public AsyncLoadingAssetListComponent asyncLoadingAssetList;
        }
    }
}