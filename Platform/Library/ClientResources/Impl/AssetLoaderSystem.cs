using System.Collections.Generic;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetLoaderSystem : ECSSystem {
        ILog log;

        [OnEventFire]
        public void ProcessAssetRequest(NodeAddedEvent e, [Combine] AssetRequestNode node, DatabaseNode db) {
            string assetGuid = node.assetReference.Reference.AssetGuid;
            AssetBundleDatabase assetBundleDatabase = db.assetBundleDatabase.AssetBundleDatabase;
            AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(assetGuid);
            Entity entity = node.Entity;

            if (db.assetStorage.Contains(assetGuid)) {
                Object data = db.assetStorage.Get(assetGuid);
                AttachResourceToEntity(data, assetInfo.ObjectName, entity);
            } else {
                HashSet<AssetBundleInfo> hashSet = new();
                CollectBundles(assetInfo, hashSet);
                PrepareLoadingRequest(entity, hashSet);
            }
        }

        [OnEventFire]
        public void ProcessAssetListRequest(NodeAddedEvent e, [Combine] SingleNode<AssetReferenceListComponent> node,
            DatabaseNode db) {
            AssetBundleDatabase assetBundleDatabase = db.assetBundleDatabase.AssetBundleDatabase;
            AssetStorageComponent assetStorage = db.assetStorage;
            IEnumerable<AssetReference> references = node.component.References;
            List<Object> assetList;

            if (FillAllAssetsFromStorage(references, assetStorage, out assetList)) {
                node.Entity.AddComponent(new ResourceDataListComponent(assetList));
                return;
            }

            HashSet<AssetBundleInfo> hashSet = new();

            foreach (AssetReference item in references) {
                AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(item.AssetGuid);
                CollectBundles(assetInfo, hashSet);
            }

            PrepareLoadingRequest(node.Entity, hashSet);
        }

        void PrepareLoadingRequest(Entity request, HashSet<AssetBundleInfo> bundleInfos) {
            request.AddComponent(new ResourceGroupComponent(request));
            AssetBundlesLoadDataComponent assetBundlesLoadDataComponent = new();
            assetBundlesLoadDataComponent.AllBundles = bundleInfos;
            assetBundlesLoadDataComponent.BundlesToLoad = new HashSet<AssetBundleInfo>(bundleInfos);
            assetBundlesLoadDataComponent.LoadingBundles = new HashSet<AssetBundleInfo>();
            assetBundlesLoadDataComponent.LoadedBundles = new Dictionary<string, AssetBundle>();
            request.AddComponent(assetBundlesLoadDataComponent);
            request.AddComponent<ResourceLoadStatComponent>();
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

        void CollectBundles(AssetInfo info, ICollection<AssetBundleInfo> dependencies) {
            dependencies.Add(info.AssetBundleInfo);

            foreach (AssetBundleInfo allDependency in info.AssetBundleInfo.AllDependencies) {
                dependencies.Add(allDependency);
            }
        }

        [OnEventComplete]
        public void CompleteLoadAssetFromBundle(AssetBundlesLoadedEvent e, AssetDependenciesNode assetNode,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            AssetBundleDatabase assetBundleDatabase = db.component.AssetBundleDatabase;
            AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(assetNode.assetReference.Reference.AssetGuid);
            Entity entity = assetNode.Entity;
            Object @object = ResolveAsset(assetInfo, assetNode.assetBundlesLoadData.LoadedBundles);
            AttachResourceToEntity(@object, assetInfo.ObjectName, entity);
            assetNode.assetReference.Reference.SetReference(@object);
            CleanLoadingRequest(assetNode.Entity);
        }

        [OnEventComplete]
        public void CompleteLoadAssetListFromBundles(AssetBundlesLoadedEvent e, AssetListDependenciesNode assetList,
            [JoinAll] SingleNode<AssetBundleDatabaseComponent> db) {
            AssetBundleDatabase assetBundleDatabase = db.component.AssetBundleDatabase;
            IEnumerable<AssetReference> references = assetList.assetReferenceList.References;
            List<Object> list = new();

            foreach (AssetReference item in references) {
                AssetInfo assetInfo = assetBundleDatabase.GetAssetInfo(item.AssetGuid);
                Object @object = ResolveAsset(assetInfo, assetList.assetBundlesLoadData.LoadedBundles);
                list.Add(@object);
                item.SetReference(@object);
            }

            ResourceDataListComponent resourceDataListComponent = new();
            resourceDataListComponent.DataList = list;
            assetList.Entity.AddComponent(resourceDataListComponent);
            CleanLoadingRequest(assetList.Entity);
        }

        public void AttachResourceToEntity(Object data, string name, Entity entity) {
            ResourceDataComponent resourceDataComponent = new();
            resourceDataComponent.Data = data;
            resourceDataComponent.Name = name;
            entity.AddComponent(resourceDataComponent);
        }

        void CleanLoadingRequest(Entity entity) {
            if (entity.HasComponent<AssetBundlesLoadDataComponent>()) {
                entity.RemoveComponent<AssetBundlesLoadDataComponent>();
            }

            if (entity.HasComponent<ResourceGroupComponent>()) {
                entity.RemoveComponent<ResourceGroupComponent>();
            }

            if (entity.HasComponent<ResourceLoadStatComponent>()) {
                entity.RemoveComponent<ResourceLoadStatComponent>();
            }
        }

        Object ResolveAsset(AssetInfo info, Dictionary<string, AssetBundle> cache) {
            AssetBundle assetBundle = cache[info.AssetBundleInfo.BundleName];
            GetLogger().InfoFormat("LoadAsset objectName={0} objectType={1}", info.ObjectName, info.AssetType);
            return assetBundle.LoadAsset(info.ObjectName, info.AssetType);
        }

        ILog GetLogger() {
            if (log == null) {
                log = LoggerProvider.GetLogger(this);
            }

            return log;
        }

        public class AssetRequestNode : Node {
            public AssetReferenceComponent assetReference;

            public AssetRequestComponent assetRequest;
        }

        [Not(typeof(PreloadComponent))]
        public class AssetDependenciesNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetReferenceComponent assetReference;
        }

        [Not(typeof(PreloadComponent))]
        public class AssetListDependenciesNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetReferenceListComponent assetReferenceList;
        }

        public class DatabaseNode : Node {
            public AssetBundleDatabaseComponent assetBundleDatabase;

            public AssetStorageComponent assetStorage;
        }
    }
}