using System;
using System.Collections.Generic;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleDiskCacheSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<AssetBundleDatabaseComponent> dbNode,
            SingleNode<BaseUrlComponent> baseUrlNode) {
            AssetBundleDiskCache assetBundleDiskCache = new(dbNode.component.AssetBundleDatabase, baseUrlNode.component.Url);
            assetBundleDiskCache.CleanOldBundlesCache();

            if (!EnoughFreeSpaceForCache(assetBundleDiskCache)) {
                SheduleErrorEvent<NotEnoughDiskSpaceForCacheErrorEvent>(dbNode.Entity, "Not enough disk space for cache");
                return;
            }

            baseUrlNode.Entity.AddComponent(new AssetBundleDiskCacheComponent {
                AssetBundleDiskCache = assetBundleDiskCache
            });
        }

        bool EnoughFreeSpaceForCache(AssetBundleDiskCache cache) {
            long num = cache.RequiredFreeSpaceInBytes();
            Console.WriteLine("AssetBundleDiskCache: requiredFreeSpace: " + num + " bytes.");
            int num2 = (int)(num / 314572800);
            long length = num - 314572800L * num2 + 52428800;
            num2++;
            List<FileStream> list = new();

            try {
                for (int i = 0; i < num2; i++) {
                    string testFileName = GetTestFileName(cache.CachePath, i);
                    list.Add(File.Open(testFileName, FileMode.OpenOrCreate));
                }
            } catch (Exception) {
                return true;
            }

            bool result = true;

            try {
                for (int j = 0; j < num2 - 1; j++) {
                    list[j].SetLength(314572800L);
                }

                list[num2 - 1].SetLength(length);
            } catch (IOException) {
                result = false;
            } finally {
                for (int k = 0; k < num2; k++) {
                    list[k].Close();
                    File.Delete(GetTestFileName(cache.CachePath, k));
                }
            }

            return result;
        }

        string GetTestFileName(string cachePath, int index) => cachePath + "/testFreeSpace.test" + index;

        [OnEventFire]
        public void StartLoad(NodeAddedEvent e, SingleNode<AssetBundleLoadingComponent> loadingNode,
            [JoinAll] SingleNode<AssetBundleDiskCacheComponent> cacheNode) {
            AssetBundleDiskCache assetBundleDiskCache = cacheNode.component.AssetBundleDiskCache;
            AssetBundleLoadingComponent component = loadingNode.component;
            AssetBundleInfo info = component.Info;
            component.AssetBundleDiskCacheRequest = assetBundleDiskCache.GetFromCacheOrDownload(info);
            component.StartTime = Time.realtimeSinceStartup;
        }

        [OnEventFire]
        public void Update(UpdateEvent e, SingleNode<AssetBundleDiskCacheComponent> cacheNode,
            [JoinAll] ICollection<SingleNode<AssetBundleLoadingComponent>> loadingBundleNodes) {
            AssetBundleDiskCache assetBundleDiskCache = cacheNode.component.AssetBundleDiskCache;
            assetBundleDiskCache.Update();

            foreach (SingleNode<AssetBundleLoadingComponent> loadingBundleNode in loadingBundleNodes) {
                AssetBundleDiskCacheRequest assetBundleDiskCacheRequest =
                    loadingBundleNode.component.AssetBundleDiskCacheRequest;

                if (assetBundleDiskCacheRequest.IsDone) {
                    if (!string.IsNullOrEmpty(assetBundleDiskCacheRequest.Error)) {
                        HandleError(loadingBundleNode.Entity,
                            assetBundleDiskCacheRequest,
                            assetBundleDiskCacheRequest.Error);

                        break;
                    }

                    ScheduleEvent<LoadCompleteEvent>(loadingBundleNode.Entity);
                }
            }
        }

        void HandleError(Entity entity, AssetBundleDiskCacheRequest request, string errorMessage) {
            if (request.Progress > 0f && request.Progress < 1f) {
                SheduleErrorEvent<ServerDisconnectedEvent>(entity, errorMessage);
            } else {
                SheduleErrorEvent<NoServerConnectionEvent>(entity, errorMessage);
            }
        }

        void SheduleErrorEvent<T>(Entity entity, string errorMessage) where T : Event, new() {
            Log.Error(errorMessage);
            ScheduleEvent<T>(entity);
        }
    }
}