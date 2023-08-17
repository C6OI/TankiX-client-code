using System.Collections.Generic;
using Assets.platform.library.ClientResources.Scripts.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundlePreloadSystem : ECSSystem {
        const string ASSET_PRIORITY_CONFIG = "clientlocal/clientresources/assetpriority";

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        [OnEventFire]
        public void StartPreload(NodeAddedEvent e, SingleNode<PreloadAllResourcesComponent> preload,
            [JoinAll] DataBaseNode db) {
            if (!DiskCaching.Enabled) {
                return;
            }

            AssetBundleDatabase assetBundleDatabase = db.assetBundleDatabase.AssetBundleDatabase;
            AssetBundleDiskCache assetBundleDiskCache = db.assetBundleDiskCache.AssetBundleDiskCache;
            List<string> prioritizedAssetsConfigPathList = GetPrioritizedAssetsConfigPathList();
            int num = 100 + prioritizedAssetsConfigPathList.Count;
            List<string> list = new();

            for (int i = 0; i < prioritizedAssetsConfigPathList.Count; i++) {
                AssetReferenceComponent assetReference = GetAssetReference(prioritizedAssetsConfigPathList[i]);
                string assetGuid = assetReference.Reference.AssetGuid;
                list.Add(assetGuid);
                AssetBundleInfo assetBundleInfoByGuid = assetBundleDatabase.GetAssetBundleInfoByGuid(assetGuid);

                if (!assetBundleDiskCache.IsCached(assetBundleInfoByGuid)) {
                    int loadingPriority = num - i;
                    CreateEntityForPreloadingBundles(assetReference, loadingPriority);
                }
            }

            foreach (string rootGuid in assetBundleDatabase.GetRootGuids()) {
                AssetBundleInfo assetBundleInfoByGuid2 = assetBundleDatabase.GetAssetBundleInfoByGuid(rootGuid);

                if (!list.Contains(rootGuid) && !assetBundleDiskCache.IsCached(assetBundleInfoByGuid2)) {
                    AssetReferenceComponent assetReferenceComponent = new(new AssetReference(rootGuid));
                    CreateEntityForPreloadingBundles(assetReferenceComponent, 0);
                }
            }
        }

        [OnEventComplete]
        public void CompletePreload(UpdateEvent e, SingleNode<PreloadAllResourcesComponent> preload,
            [JoinAll] ICollection<PreloadNode> loadingRequests) {
            if (loadingRequests.Count == 0) {
                preload.Entity.RemoveComponent<PreloadAllResourcesComponent>();
            }
        }

        [OnEventFire]
        public void CancelPreload(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> preload,
            [JoinAll] ICollection<PreloadNode> loadingRequests) {
            foreach (PreloadNode loadingRequest in loadingRequests) {
                if (loadingRequest.assetBundlesLoadData.LoadingBundles.Count > 0) {
                    loadingRequest.assetBundlesLoadData.BundlesToLoad.Clear();
                } else {
                    DeleteEntity(loadingRequest.Entity);
                }
            }
        }

        [OnEventComplete]
        public void Complete(AssetBundlesLoadedEvent e, PreloadNode loadingRequest) => DeleteEntity(loadingRequest.Entity);

        void CreateEntityForPreloadingBundles(AssetReferenceComponent assetReferenceComponent, int loadingPriority) {
            Entity entity = CreateEntity("PreloadBundles");
            entity.AddComponent(assetReferenceComponent);
            entity.AddComponent<PreloadComponent>();
            LoadAssetBundlesRequestComponent loadAssetBundlesRequestComponent = new();
            loadAssetBundlesRequestComponent.LoadingPriority = loadingPriority;
            LoadAssetBundlesRequestComponent component = loadAssetBundlesRequestComponent;
            entity.AddComponent(component);
        }

        static List<string> GetPrioritizedAssetsConfigPathList() {
            YamlNode config = ConfigurationService.GetConfig("clientlocal/clientresources/assetpriority");

            ConfigPathCollectionComponent configPathCollectionComponent =
                config.GetChildNode("configPathCollection").ConvertTo<ConfigPathCollectionComponent>();

            return configPathCollectionComponent.Collection;
        }

        static AssetReferenceComponent GetAssetReference(string configPath) {
            YamlNode config = ConfigurationService.GetConfig(configPath);
            return config.GetChildNode("unityAsset").ConvertTo<AssetReferenceComponent>();
        }

        public class PreloadNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetReferenceComponent assetReference;

            public PreloadComponent preload;
        }

        public class DataBaseNode : Node {
            public AssetBundleDatabaseComponent assetBundleDatabase;

            public AssetBundleDiskCacheComponent assetBundleDiskCache;

            public BaseUrlComponent baseUrl;
        }
    }
}