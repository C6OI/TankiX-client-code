using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    [Serializable]
    public class AssetBundleInfo {
        [SerializeField] string bundleName;

        [SerializeField] string hash;

        [SerializeField] uint crc;

        [SerializeField] uint cacheCrc;

        [SerializeField] int size;

        [SerializeField] List<string> dependenciesNames = new();

        [SerializeField] List<AssetInfo> assets = new();

        [SerializeField] int modificationHash;

        [NonSerialized] List<AssetBundleInfo> dependencies = new();

        [NonSerialized] bool isCached;

        public bool IsCached {
            get => isCached;
            set => isCached = value;
        }

        public string BundleName {
            get => bundleName;
            set => bundleName = value;
        }

        public Hash128 Hash {
            get => Hash128.Parse(hash);
            set => hash = value.ToString();
        }

        public string HashString => hash;

        public int ModificationHash {
            get => modificationHash;
            set => modificationHash = value;
        }

        public int Size {
            get => size;
            set => size = value;
        }

        public List<AssetInfo> Assets {
            get => assets;
            set => assets = value;
        }

        public List<string> DependenciesNames {
            get => dependenciesNames;
            set => dependenciesNames = value;
        }

        public List<AssetBundleInfo> Dependencies {
            get => dependencies;
            set => dependencies = value;
        }

        public ICollection<AssetBundleInfo> AllDependencies {
            get {
                List<AssetBundleInfo> list = new();
                CollectDependencies(list);
                return list;
            }
        }

        public string Filename => AssetBundleNaming.AddCrcToBundleName(BundleName, CRC);

        public uint CRC {
            get => crc;
            set => crc = value;
        }

        public uint CacheCRC {
            get => cacheCrc;
            set => cacheCrc = value;
        }

        internal void AddAssetInfo(AssetInfo assetInfo) {
            Assets.Add(assetInfo);
        }

        void CollectDependencies(ICollection<AssetBundleInfo> collector) {
            List<AssetBundleInfo> list = Dependencies;

            foreach (AssetBundleInfo item in list) {
                collector.Add(item);
            }
        }

        public override string ToString() => string.Format("[AssetBundleInfo: bundleName={0}, hash={1}, size={2}, dependenciesNames={3}, assets={4}]", bundleName, hash, size,
            dependenciesNames, assets);

        public override int GetHashCode() => Filename.GetHashCode();

        public override bool Equals(object obj) {
            AssetBundleInfo assetBundleInfo = obj as AssetBundleInfo;

            if (assetBundleInfo == null) {
                return false;
            }

            return Filename == assetBundleInfo.Filename;
        }
    }
}