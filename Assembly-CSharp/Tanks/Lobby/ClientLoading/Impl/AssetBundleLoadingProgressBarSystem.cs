using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class AssetBundleLoadingProgressBarSystem : ECSSystem {
        public static float PROGRESS_LOADING_TIMEOUT = 120f;

        readonly List<AssetBundleInfo> canceledBundles = new();

        float lastProgress;

        float lastUpdateTime;

        readonly HashSet<AssetBundleInfo> loadingBundles = new();

        [OnEventFire]
        public void PrepareProgressDelayed(NodeAddedEvent e, ProgressBarNode node, SingleNode<AssetBundleDatabaseComponent> assetDb) {
            NewEvent<ProgressBarCalucationEvent>().Attach(node.Entity).ScheduleDelayed(node.resourcesLoadProgressBar.TimeBeforeProgressCalculation);
        }

        [OnEventFire]
        public void PrepareLoadTask(ProgressBarCalucationEvent e, ProgressBarNode node, [JoinAll] ICollection<AssetBundleNode> loadingBundles) {
            HashSet<AssetBundleInfo> hashSet = new();

            foreach (AssetBundleNode loadingBundle in loadingBundles) {
                foreach (AssetBundleInfo item in loadingBundle.assetBundlesLoadData.BundlesToLoad) {
                    if (!AssetBundlesStorage.IsStored(item)) {
                        hashSet.Add(item);
                    }
                }

                foreach (AssetBundleInfo loadingBundle2 in loadingBundle.assetBundlesLoadData.LoadingBundles) {
                    if (!AssetBundlesStorage.IsStored(loadingBundle2)) {
                        hashSet.Add(loadingBundle2);
                    }
                }
            }

            LoadBundlesTaskComponent component = new(hashSet);
            node.Entity.AddComponent(component);
        }

        [OnEventComplete]
        public void UpdateProgress(UpdateEvent e, CalculatedProgressBarNode node, [JoinAll] ICollection<SingleNode<ResourceLoadStatComponent>> loadStats,
            [JoinAll] ICollection<SingleNode<AssetBundleLoadingComponent>> assetBundlesLoading) {
            LoadBundlesTaskComponent loadBundlesTask = node.loadBundlesTask;
            UpdateLoadBundlesTask(loadBundlesTask, loadStats);
            node.loadBundlesTaskProvider.UpdateData(loadBundlesTask);

            if (loadBundlesTask.AllBundlesLoaded()) {
                Log.Info("ProgressComplete");
                node.Entity.RemoveComponent<LoadBundlesTaskComponent>();
                node.Entity.AddComponentIfAbsent<LoadProgressTaskCompleteComponent>();
            } else if (!node.Entity.HasComponent<LoadProgressTaskCompleteComponent>() &&
                       ShowLogIfTimeout(node.resourcesLoadProgressBar.ProgressBar.ProgressValue, loadBundlesTask, assetBundlesLoading)) {
                Log.Info("LoadingComplete by timeout");
                node.Entity.AddComponent<LoadProgressTaskCompleteComponent>();
            }
        }

        [OnEventFire]
        public void DisableBackButton(NodeAddedEvent e, SingleNode<ResourcesLoadProgressBarComponent> screen, [JoinAll] [Combine] SingleNode<BackButtonComponent> backButton) {
            backButton.component.Disabled = true;
        }

        void UpdateLoadBundlesTask(LoadBundlesTaskComponent loadBundlesTask, ICollection<SingleNode<ResourceLoadStatComponent>> loadStats) {
            HashSet<AssetBundleInfo> trackedBundles = loadBundlesTask.TrackedBundles;
            AssetBundleInfo[] array = loadBundlesTask.TrackedBundles.ToArray();

            foreach (AssetBundleInfo assetBundleInfo in array) {
                if (AssetBundlesStorage.IsStored(assetBundleInfo)) {
                    loadBundlesTask.MarkBundleAsLoaded(assetBundleInfo);
                }
            }

            CancelUnactualBundles(loadBundlesTask, loadStats);

            foreach (SingleNode<ResourceLoadStatComponent> loadStat in loadStats) {
                foreach (KeyValuePair<AssetBundleInfo, float> item in loadStat.component.BundleToProgress) {
                    AssetBundleInfo key = item.Key;

                    if (trackedBundles.Contains(key)) {
                        float value = item.Value;
                        loadBundlesTask.SetBundleProgressLoading(key, value);
                    }
                }
            }
        }

        void CancelUnactualBundles(LoadBundlesTaskComponent loadBundlesTask, ICollection<SingleNode<ResourceLoadStatComponent>> loadStats) {
            HashSet<AssetBundleInfo> trackedBundles = loadBundlesTask.TrackedBundles;
            loadingBundles.Clear();
            canceledBundles.Clear();

            foreach (SingleNode<ResourceLoadStatComponent> loadStat in loadStats) {
                foreach (AssetBundleInfo key in loadStat.component.BundleToProgress.Keys) {
                    loadingBundles.Add(key);
                }
            }

            foreach (AssetBundleInfo item in trackedBundles) {
                if (!loadingBundles.Contains(item)) {
                    canceledBundles.Add(item);
                }
            }

            foreach (AssetBundleInfo canceledBundle in canceledBundles) {
                loadBundlesTask.CancelBundle(canceledBundle);
            }
        }

        bool ShowLogIfTimeout(float progress, LoadBundlesTaskComponent loadBundlesTask, ICollection<SingleNode<AssetBundleLoadingComponent>> assetBundleLoading) {
            bool result = true;

            if (progress != lastProgress) {
                lastUpdateTime = Time.realtimeSinceStartup;
            } else if (Time.realtimeSinceStartup - lastUpdateTime > PROGRESS_LOADING_TIMEOUT) {
                lastUpdateTime = Time.realtimeSinceStartup;
                Console.WriteLine("Timeout on tracked bundles loading");
                Console.WriteLine("Tracked bundles:");

                foreach (AssetBundleInfo trackedBundle in loadBundlesTask.TrackedBundles) {
                    Console.WriteLine(trackedBundle.Filename);
                }

                Console.WriteLine("ResourceLoadStats:");

                foreach (SingleNode<AssetBundleLoadingComponent> item in assetBundleLoading) {
                    Console.WriteLine("LoadingBundle: {0}, state: {1}", item.component.AssetBundleDiskCacheRequest.AssetBundleInfo, item.component.AssetBundleDiskCacheRequest.State);
                }

                result = false;
            }

            lastProgress = progress;
            return result;
        }

        public class AssetBundleNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }

        public class ProgressBarNode : Node {
            public LoadBundlesTaskProviderComponent loadBundlesTaskProvider;
            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;
        }

        public class CalculatedProgressBarNode : Node {
            public LoadBundlesTaskComponent loadBundlesTask;

            public LoadBundlesTaskProviderComponent loadBundlesTaskProvider;
            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;
        }

        public class ProgressBarCalucationEvent : Event { }
    }
}