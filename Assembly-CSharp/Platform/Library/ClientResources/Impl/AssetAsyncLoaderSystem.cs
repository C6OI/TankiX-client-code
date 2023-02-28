using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public void StartLoadAssetFromBundle(AssetBundlesLoadedEvent e, AssetDependenciesNode asset, [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            LoadAssetFromBundleRequest request = CreateLoadAssetRequest(asset.assetReference.Reference, db.component, asset.assetBundlesLoadData);
            AsyncLoadingAssetComponent asyncLoadingAssetComponent = new();
            asyncLoadingAssetComponent.Request = request;
            AsyncLoadingAssetComponent component = asyncLoadingAssetComponent;
            asset.Entity.AddComponent(component);
            asset.Entity.RemoveComponentIfPresent<LoadAssetBundlesRequestComponent>();
        }

        [OnEventFire]
        public void StartLoadAsset(UpdateEvent e, AsyncLoadingAssetNode loadingAsset, [JoinAll] Optional<SingleNode<TanyaSleepComponent>> tanya) {
            if (!tanya.IsPresent() && !loadingAsset.asyncLoadingAsset.Request.IsStarted) {
                loadingAsset.asyncLoadingAsset.Request.StartExecute();
            }
        }

        [OnEventFire]
        public void CompleteLoadAssetFromBundle(UpdateEvent e, AsyncLoadingAssetNode loadingAsset, [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
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
        public void CancelAssetBundleLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest,
            [JoinSelf] SingleNode<LoadAssetBundlesRequestComponent> loadAssetBundlesRequest) {
            assetRequest.Entity.RemoveComponent<LoadAssetBundlesRequestComponent>();
        }

        [OnEventFire]
        public void CancelAssetLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest, [JoinSelf] SingleNode<AsyncLoadingAssetComponent> loadAssetBundlesRequest) {
            assetRequest.Entity.RemoveComponent<AsyncLoadingAssetComponent>();
        }

        [OnEventFire]
        public void CancelAssetListLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest,
            [JoinSelf] SingleNode<AsyncLoadingAssetListComponent> loadAssetBundlesRequest) {
            assetRequest.Entity.RemoveComponent<AsyncLoadingAssetListComponent>();
        }

        static AsyncLoadAssetFromBundleRequest CreateLoadAssetRequest(AssetReference assetReference, AssetBundleDatabaseComponent db, AssetBundlesLoadDataComponent assetBundlesLoadData) {
            AssetBundleDatabase assetBundleDatabase = db.AssetBundleDatabase;
            AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(assetReference.AssetGuid);

            try {
                AssetBundle bundle = assetBundlesLoadData.LoadedBundles[assetInfo.AssetBundleInfo];
                return new AsyncLoadAssetFromBundleRequest(bundle, assetInfo.ObjectName, assetInfo.AssetType);
            } catch (KeyNotFoundException innerException) {
                throw new Exception("Bundle not found in LoadedBundles: " + assetInfo.AssetBundleInfo.BundleName + ", ref=" + assetReference.AssetGuid, innerException);
            }
        }

        bool FillAllAssetsFromStorage(IEnumerable<AssetReference> referencies, AssetStorageComponent storage, out List<Object> assetList) {
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

        public class AssetDependenciesNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetReferenceComponent assetReference;

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
    }
}