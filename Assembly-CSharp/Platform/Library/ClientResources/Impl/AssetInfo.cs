using System;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    [Serializable]
    public class AssetInfo {
        [SerializeField] string guid;

        [SerializeField] string objectName;

        [SerializeField] int typeHash;

        [NonSerialized] AssetBundleInfo assetBundleInfo;

        public Type AssetType => AssetTypeRegistry.GetType(typeHash);

        public string Guid {
            get => guid;
            set => guid = value;
        }

        public string ObjectName {
            get => objectName;
            set => objectName = value;
        }

        public AssetBundleInfo AssetBundleInfo {
            get => assetBundleInfo;
            set => assetBundleInfo = value;
        }

        internal int TypeHash {
            get => typeHash;
            set => typeHash = value;
        }

        public override string ToString() =>
            string.Format("[AssetInfo: guid={0}, objectName={1}, Type={2}, assetBundleName={3}]", guid, objectName, AssetType, assetBundleInfo.BundleName);
    }
}