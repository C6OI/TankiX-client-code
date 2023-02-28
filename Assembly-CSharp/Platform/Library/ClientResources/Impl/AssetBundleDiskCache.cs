using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleDiskCache : IDisposable {
        public static int SIZE_BEFORE_SYSTEM_GC = 52428800;

        public static readonly string CACHE_DIRECTORY = "AssetBundlesCache";

        public static AssetBundleDiskCache INSTANCE;

        public static readonly int MINIMUM_PROJECT_BUNDLES = 100;

        readonly DiskCacheWriterThread diskWriter;

        readonly Thread diskWriterThread;

        readonly LinkedList<AssetBundleDiskCacheTask> tasks = new();

        public AssetBundleDiskCache(AssetBundleDatabase dataBase, string baseUrl) {
            DataBase = dataBase;
            BaseUrl = baseUrl;
            CachePath = ResolveAssetBundlesCachePath();
            diskWriter = new DiskCacheWriterThread();
            diskWriterThread = new Thread(diskWriter.Run);
            diskWriterThread.Start();
            INSTANCE = this;
        }

        public AssetBundleDatabase DataBase { get; }

        public string CachePath { get; }

        public string BaseUrl { get; }

        public void Dispose() {
            if (diskWriter != null) {
                diskWriter.Stop();
            }

            foreach (AssetBundleDiskCacheTask task in tasks) {
                task.Dispose();
            }

            tasks.Clear();
        }

        public static void Clean() {
            if (INSTANCE != null) {
                INSTANCE.Dispose();
            }
        }

        ~AssetBundleDiskCache() {
            Dispose();
        }

        public AssetBundleDiskCacheRequest GetFromCacheOrDownload(AssetBundleInfo info) {
            AssetBundleDiskCacheRequest assetBundleDiskCacheRequest = new(info);
            tasks.AddLast(new AssetBundleDiskCacheTask(this).Init(assetBundleDiskCacheRequest));
            return assetBundleDiskCacheRequest;
        }

        public void Update() {
            LinkedListNode<AssetBundleDiskCacheTask> linkedListNode = tasks.First;

            while (linkedListNode != null) {
                LinkedListNode<AssetBundleDiskCacheTask> next = linkedListNode.Next;
                AssetBundleDiskCacheTask value = linkedListNode.Value;

                if (value.Run()) {
                    int size = value.AssetBundleInfo.Size;
                    value.Dispose();
                    linkedListNode.Value = null;
                    tasks.Remove(linkedListNode);

                    if (size > SIZE_BEFORE_SYSTEM_GC) {
                        GC.Collect();
                    }
                }

                linkedListNode = next;
            }
        }

        public void CleanOldBundlesCache() {
            HashSet<string> currentVersionBundleNames = GetCurrentVersionBundleNames();

            if (currentVersionBundleNames.Count < MINIMUM_PROJECT_BUNDLES) {
                Console.WriteLine("AssetBundle database looks incorrect, skip cleaning old bundles");
                return;
            }

            foreach (string file in FileUtils.GetFiles(CachePath, bundleName => bundleName.Contains(".bundle"))) {
                string fileName = Path.GetFileName(file);

                if (!currentVersionBundleNames.Contains(fileName)) {
                    try {
                        File.SetAttributes(file, FileAttributes.Archive);
                        File.Delete(file);
                    } catch (IOException ex) {
                        LoggerProvider.GetLogger<AssetBundleDiskCache>().ErrorFormat("Can't delete old bundle {0}, IOException: {1}", fileName, ex.Message);
                    } catch (UnauthorizedAccessException ex2) {
                        LoggerProvider.GetLogger<AssetBundleDiskCache>().ErrorFormat("Can't delete old bundle {0}, UnauthorizedAccessException: {1}", fileName, ex2.Message);
                    }
                }
            }
        }

        public bool CleanCache(AssetBundleInfo info) {
            try {
                string assetBundleCachePath = GetAssetBundleCachePath(info);
                File.SetAttributes(assetBundleCachePath, FileAttributes.Archive);
                File.Delete(assetBundleCachePath);
            } catch (IOException) {
                return false;
            }

            return true;
        }

        public HashSet<string> GetCurrentVersionBundleNames() {
            HashSet<string> hashSet = new();

            foreach (AssetBundleInfo allAssetBundle in DataBase.GetAllAssetBundles()) {
                hashSet.Add(allAssetBundle.Filename);
            }

            return hashSet;
        }

        public bool IsCached(AssetBundleInfo info) => File.Exists(GetAssetBundleCachePath(info));

        public string GetAssetBundleCachePath(AssetBundleInfo info) => string.Format("{0}/{1}", CachePath, info.Filename);

        public string ResolveAssetBundlesCachePath() {
            string dataPath = Application.dataPath;
            string name = BuildTargetName.GetName();
            string text = string.Format("{0}/{1}/{2}/", dataPath, CACHE_DIRECTORY, name);

            try {
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }

                string path = string.Format("{0}{1:x8}.test", text, Random.Range(0, int.MaxValue));

                using (new FileStream(path, FileMode.OpenOrCreate)) { }

                File.Delete(path);
                return text;
            } catch {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                text = string.Format("{0}/TankiX/tankix_Data/{1}/{2}/", folderPath, CACHE_DIRECTORY, name);

                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                    return text;
                }

                return text;
            }
        }

        public DiskCacheWriterRequest WriteToDiskCache(string path, byte[] data) => diskWriter.Write(path, data);

        public long RequiredFreeSpaceInBytes() {
            List<AssetBundleInfo> allAssetBundles = DataBase.GetAllAssetBundles();
            long num = 0L;

            foreach (AssetBundleInfo item in allAssetBundles) {
                if (!IsCached(item)) {
                    num += item.Size;
                }
            }

            return num;
        }
    }
}