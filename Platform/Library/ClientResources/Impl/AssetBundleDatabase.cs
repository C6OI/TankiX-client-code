using System.Collections.Generic;
using System.Linq;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleDatabase {
        readonly Dictionary<string, AssetBundleInfo> guidToAssetBundleInfo = new();
        [SerializeField] List<AssetBundleInfo> bundles = new();

        Dictionary<string, AssetInfo> guidToAssetInfo = new();

        [SerializeField] List<string> rootGuids;

        public string Serialize() {
            foreach (AssetBundleInfo bundle in bundles) {
                bundle.DependenciesNames.Clear();

                foreach (AssetBundleInfo dependency in bundle.Dependencies) {
                    bundle.DependenciesNames.Add(dependency.BundleName);
                }
            }

            return JsonUtility.ToJson(this, true);
        }

        public void Deserialize(string data) {
            Clear();
            JsonUtility.FromJsonOverwrite(data, this);
            guidToAssetInfo.Clear();

            foreach (AssetBundleInfo bundle in bundles) {
                foreach (AssetInfo asset in bundle.Assets) {
                    asset.AssetBundleInfo = bundle;
                    guidToAssetInfo.Add(asset.Guid, asset);
                    guidToAssetBundleInfo.Add(asset.Guid, bundle);
                }
            }

            foreach (AssetBundleInfo bundle2 in bundles) {
                bundle2.Dependencies.Clear();
                string dependencyName;

                foreach (string dependenciesName in bundle2.DependenciesNames) {
                    dependencyName = dependenciesName;
                    bundle2.Dependencies.Add(bundles.Single(b => dependencyName.Equals(b.BundleName)));
                }
            }
        }

        public void Clear() {
            bundles = new List<AssetBundleInfo>();
            rootGuids = null;
            guidToAssetInfo = new Dictionary<string, AssetInfo>();
        }

        public bool IsAssetRegistered(string guid) => guidToAssetInfo.ContainsKey(guid);

        public AssetInfo GetAssetInfo(string guid) {
            AssetInfo value;

            if (guidToAssetInfo.TryGetValue(guid, out value)) {
                return value;
            }

            throw new AssetNotFoundException(guid);
        }

        public List<string> GetRootGuids() => rootGuids;

        public void SetRootGuids(List<string> rootGuids) => this.rootGuids = rootGuids;

        public AssetBundleInfo GetAssetBundleInfoByName(string assetBundleName) {
            int? assetBundleIndex = GetAssetBundleIndex(assetBundleName);
            return !assetBundleIndex.HasValue ? null : bundles[assetBundleIndex.Value];
        }

        public AssetBundleInfo GetAssetBundleInfoByGuid(string guid) => guidToAssetBundleInfo[guid];

        AssetBundleInfo CreateAssetBundleInfo(string assetBundleName) {
            AssetBundleInfo assetBundleInfo = new();
            assetBundleInfo.BundleName = assetBundleName;
            bundles.Add(assetBundleInfo);
            return assetBundleInfo;
        }

        public void AddAsset(string assetBundleName, string assetObjectName, string assetGuid, string assetExtension) {
            AssetBundleInfo orCreateAssetBundleInfo = GetOrCreateAssetBundleInfo(assetBundleName);
            AssetInfo assetInfo = new();
            assetInfo.AssetBundleInfo = orCreateAssetBundleInfo;
            assetInfo.ObjectName = assetObjectName;
            assetInfo.Guid = assetGuid;
            assetInfo.TypeHash = AssetTypeRegistry.GetTypeHashByExtension(assetExtension);
            orCreateAssetBundleInfo.AddAssetInfo(assetInfo);
            guidToAssetInfo.Add(assetGuid, assetInfo);
        }

        public void AddDependency(string assetBundleName, string dependencyAssetBundleName) {
            int? assetBundleIndex = GetAssetBundleIndex(assetBundleName);

            if (!assetBundleIndex.HasValue) {
                Debug.LogError("Bundle not registred " + assetBundleName);
                return;
            }

            AssetBundleInfo assetBundleInfo = bundles[assetBundleIndex.Value];
            int? assetBundleIndex2 = GetAssetBundleIndex(dependencyAssetBundleName);

            if (assetBundleIndex2.HasValue) {
                AssetBundleInfo item = bundles[assetBundleIndex2.Value];
                assetBundleInfo.Dependencies.Add(item);
            } else {
                Debug.Log("Not found dependency assetbundle: " + assetBundleName + " dep:" + dependencyAssetBundleName);
            }
        }

        public List<AssetBundleInfo> GetAllAssetBundles() => bundles;

        AssetBundleInfo GetOrCreateAssetBundleInfo(string assetBundleName) {
            AssetBundleInfo assetBundleInfo = GetAssetBundleInfoByName(assetBundleName);

            if (assetBundleInfo == null) {
                assetBundleInfo = CreateAssetBundleInfo(assetBundleName);
            }

            return assetBundleInfo;
        }

        int? GetAssetBundleIndex(string assetBundleName) {
            for (int i = 0; i < bundles.Count; i++) {
                AssetBundleInfo assetBundleInfo = bundles[i];

                if (assetBundleInfo.BundleName.Equals(assetBundleName)) {
                    return i;
                }
            }

            return null;
        }

        bool IsCached(string guid, string baseUrl) {
            AssetInfo assetInfo = GetAssetInfo(guid);
            AssetBundleInfo assetBundleInfo = assetInfo.AssetBundleInfo;
            return IsAllDependenciesCached(assetBundleInfo, baseUrl);
        }

        bool IsAllDependenciesCached(AssetBundleInfo bundleInfo, string baseUrl) =>
            bundleInfo.AllDependencies.All(assetBundleInfo => IsBundleCached(assetBundleInfo, baseUrl));

        bool IsBundleCached(AssetBundleInfo bundleInfo, string baseUrl) {
            string assetBundleUrl = AssetBundleNaming.GetAssetBundleUrl(baseUrl, bundleInfo.Filename);
            return Caching.IsVersionCached(assetBundleUrl, bundleInfo.Hash);
        }
    }
}