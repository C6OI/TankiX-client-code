using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Tanks.Battle.ClientControls.API;
using Tanks.Lobby.ClientLoading.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class AssetBundleLoadingProgressBarSystem : ECSSystem {
        public static float PROGRESS_VISUAL_KOEFF = 0.95f;

        [OnEventFire]
        public void PrepareProgressDelayed(NodeAddedEvent e, ProgressBarNode node,
            SingleNode<AssetBundleDatabaseComponent> assetDb) {
            node.progressBar.ProgressValue = 0f;

            NewEvent<ProgressBarCalucationEvent>().Attach(node.Entity)
                .ScheduleDelayed(node.resourcesLoadProgressBar.TimeBeforeProgressCalculation);
        }

        [OnEventFire]
        public void PrepareLoadTask(ProgressBarCalucationEvent e, ProgressBarNode node,
            [JoinAll] ICollection<AssetBundleNode> loadingBundles) {
            HashSet<AssetBundleInfo> hashSet = new();

            foreach (AssetBundleNode loadingBundle in loadingBundles) {
                foreach (AssetBundleInfo item in loadingBundle.assetBundlesLoadData.BundlesToLoad) {
                    if (!AssetBundlesStorage.IsStored(item.BundleName)) {
                        hashSet.Add(item);
                    }
                }

                foreach (AssetBundleInfo loadingBundle2 in loadingBundle.assetBundlesLoadData.LoadingBundles) {
                    if (!AssetBundlesStorage.IsStored(loadingBundle2.BundleName)) {
                        hashSet.Add(loadingBundle2);
                    }
                }
            }

            LoadBundlesTaskComponent component = new(hashSet);
            node.Entity.AddComponent(component);
            node.progressBar.ProgressValue = 0f;
        }

        [OnEventComplete]
        public void UpdateProgress(UpdateEvent e, CalculatedProgressBarNode node,
            [JoinAll] ICollection<SingleNode<ResourceLoadStatComponent>> loadStats) {
            LoadBundlesTaskComponent loadBundlesTask = node.loadBundlesTask;
            UpdateLoadBundlesTask(loadBundlesTask, loadStats);
            ResourcesLoadProgressBarComponent resourcesLoadProgressBar = node.resourcesLoadProgressBar;

            float num = Mathf.Clamp(Time.realtimeSinceStartup - loadBundlesTask.LoadingStartTime,
                0f,
                resourcesLoadProgressBar.TimeToFakeLoad);

            float num2 = Mathf.Clamp(num / resourcesLoadProgressBar.TimeToFakeLoad, 0f, 1f);
            float num3 = num2 * resourcesLoadProgressBar.BytesToFakeLoad;
            float num4 = 0f;

            num4 = loadBundlesTask.BytesToLoad <= 0 ? num3 / resourcesLoadProgressBar.BytesToFakeLoad
                       : (loadBundlesTask.BytesLoaded + num3) /
                         (loadBundlesTask.BytesToLoad + resourcesLoadProgressBar.BytesToFakeLoad);

            float num5 = num4 * PROGRESS_VISUAL_KOEFF;

            if (node.progressBar.ProgressValue < num5) {
                node.progressBar.ProgressValue = num4 * PROGRESS_VISUAL_KOEFF;
            }

            if (loadBundlesTask.AllBundlesLoaded()) {
                node.progressBar.ProgressValue = 1f;
                node.Entity.RemoveComponent<LoadBundlesTaskComponent>();
                node.Entity.AddComponent<LoadProgressTaskCompleteComponent>();
            }
        }

        static void UpdateLoadBundlesTask(LoadBundlesTaskComponent loadBundlesTask,
            ICollection<SingleNode<ResourceLoadStatComponent>> loadStats) {
            HashSet<AssetBundleInfo> trackedBundles = loadBundlesTask.TrackedBundles;
            AssetBundleInfo[] array = loadBundlesTask.TrackedBundles.ToArray();

            foreach (AssetBundleInfo assetBundleInfo in array) {
                if (AssetBundlesStorage.IsStored(assetBundleInfo.BundleName)) {
                    loadBundlesTask.MarkBundleAsLoaded(assetBundleInfo);
                }
            }

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

        public class AssetBundleNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }

        public class ProgressBarNode : Node {
            public ProgressBarComponent progressBar;
            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;
        }

        public class CalculatedProgressBarNode : Node {
            public LoadBundlesTaskComponent loadBundlesTask;

            public ProgressBarComponent progressBar;
            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;
        }

        public class ProgressBarCalucationEvent : Event { }
    }
}