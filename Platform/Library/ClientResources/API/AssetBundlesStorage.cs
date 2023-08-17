using System.Collections.Generic;
using Platform.Library.ClientResources.Impl;
using UnityEngine;

namespace Platform.Library.ClientResources.API {
    public static class AssetBundlesStorage {
        public static int STORAGE_PREFARE_SIZE = 104857600;

        public static int EXPIRATION_TIME_SEC = 60;

        static readonly Dictionary<string, AssetBundleStorageEntry> name2entry = new();

        static readonly HashSet<string> loadingBundles = new();

        public static LinkedList<AssetBundleStorageEntry> EntryQueue { get; } = new();

        public static int Size { get; private set; }

        public static void Clean() {
            foreach (AssetBundleStorageEntry item in EntryQueue) {
                if (item.Bundle != null) {
                    item.Bundle.Unload(true);
                }
            }

            InternalClean();
        }

        public static void InternalClean() {
            EntryQueue.Clear();
            name2entry.Clear();
            loadingBundles.Clear();
            Size = 0;
        }

        public static void Refresh(string assetBundleName) => IsStored(assetBundleName);

        public static void MarkLoading(string assetBundleName) => loadingBundles.Add(assetBundleName);

        public static bool IsLoading(string assetBundleName) => loadingBundles.Contains(assetBundleName);

        public static bool IsStored(string assetBundleName) {
            if (name2entry.ContainsKey(assetBundleName)) {
                Access(assetBundleName);
                return true;
            }

            return false;
        }

        public static AssetBundle Get(string assetBundleName) => Access(assetBundleName).Bundle;

        static AssetBundleStorageEntry Access(string assetBundleName) {
            AssetBundleStorageEntry assetBundleStorageEntry = name2entry[assetBundleName];
            assetBundleStorageEntry.LastAccessTime = Time.time;
            return assetBundleStorageEntry;
        }

        public static void Store(string assetBundleName, AssetBundleInfo info, AssetBundle bundle) {
            loadingBundles.Remove(assetBundleName);
            AssetBundleStorageEntry assetBundleStorageEntry = new();
            assetBundleStorageEntry.Info = info;
            assetBundleStorageEntry.Bundle = bundle;
            assetBundleStorageEntry.LastAccessTime = Time.time;
            AssetBundleStorageEntry value = assetBundleStorageEntry;
            EntryQueue.AddLast(value);
            name2entry.Add(assetBundleName, value);
            Size += info.Size;
        }

        public static void StoreAsStatic(string assetBundleName, AssetBundleInfo info, AssetBundle bundle) {
            AssetBundleStorageEntry assetBundleStorageEntry = new();
            assetBundleStorageEntry.Info = info;
            assetBundleStorageEntry.Bundle = bundle;
            assetBundleStorageEntry.LastAccessTime = Time.time;
            AssetBundleStorageEntry value = assetBundleStorageEntry;
            name2entry.Add(assetBundleName, value);
        }

        public static bool Unload(AssetBundleStorageEntry entry) {
            entry.Bundle.Unload(false);
            EntryQueue.Remove(entry);
            name2entry.Remove(entry.Info.BundleName);
            Size -= entry.Info.Size;
            return true;
        }

        public static bool IsExpired(AssetBundleStorageEntry entry) =>
            Time.time - entry.LastAccessTime > EXPIRATION_TIME_SEC;

        public static bool IsNeedFreeSpace() => Size > STORAGE_PREFARE_SIZE;
    }
}