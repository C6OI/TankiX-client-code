using System.Collections.Generic;
using Platform.Library.ClientResources.Impl;
using UnityEngine;

namespace Platform.Library.ClientResources.API {
    public static class AssetBundlesStorage {
        public static int STORAGE_PREFARE_SIZE = 104857600;

        public static int EXPIRATION_TIME_SEC = 60;

        static readonly Dictionary<AssetBundleInfo, AssetBundleStorageEntry> bundle2entry = new();

        static readonly HashSet<AssetBundleInfo> loadingBundles = new();

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
            bundle2entry.Clear();
            loadingBundles.Clear();
            Size = 0;
        }

        public static void Refresh(AssetBundleInfo info) {
            IsStored(info);
        }

        public static void MarkLoading(AssetBundleInfo info) {
            loadingBundles.Add(info);
        }

        public static bool IsLoading(AssetBundleInfo info) => loadingBundles.Contains(info);

        public static bool IsStored(AssetBundleInfo info) {
            if (bundle2entry.ContainsKey(info)) {
                Access(info);
                return true;
            }

            return false;
        }

        public static AssetBundle Get(AssetBundleInfo info) => Access(info).Bundle;

        static AssetBundleStorageEntry Access(AssetBundleInfo info) {
            AssetBundleStorageEntry assetBundleStorageEntry = bundle2entry[info];
            assetBundleStorageEntry.LastAccessTime = Time.time;
            return assetBundleStorageEntry;
        }

        public static void Store(AssetBundleInfo info, AssetBundle bundle) {
            loadingBundles.Remove(info);
            AssetBundleStorageEntry assetBundleStorageEntry = new();
            assetBundleStorageEntry.Info = info;
            assetBundleStorageEntry.Bundle = bundle;
            assetBundleStorageEntry.LastAccessTime = Time.time;
            AssetBundleStorageEntry value = assetBundleStorageEntry;
            EntryQueue.AddLast(value);
            bundle2entry.Add(info, value);
            Size += info.Size;
        }

        public static void StoreAsStatic(AssetBundleInfo info, AssetBundle bundle) {
            AssetBundleStorageEntry assetBundleStorageEntry = new();
            assetBundleStorageEntry.Info = info;
            assetBundleStorageEntry.Bundle = bundle;
            assetBundleStorageEntry.LastAccessTime = Time.time;
            AssetBundleStorageEntry value = assetBundleStorageEntry;
            bundle2entry.Add(info, value);
        }

        public static bool Unload(AssetBundleStorageEntry entry) {
            entry.Bundle.Unload(false);
            EntryQueue.Remove(entry);
            bundle2entry.Remove(entry.Info);
            Size -= entry.Info.Size;
            return true;
        }

        public static bool IsExpired(AssetBundleStorageEntry entry) => Time.time - entry.LastAccessTime > EXPIRATION_TIME_SEC;

        public static bool IsNeedFreeSpace() => Size > STORAGE_PREFARE_SIZE;
    }
}