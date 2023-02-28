using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetLoadStatSystem : ECSSystem {
        const float EPS = 0.001f;

        [OnEventFire]
        public void CalculateLoadSize(NodeAddedEvent e, LoadStatNode node, SingleNode<AssetBundleDatabaseComponent> db) {
            int num = 0;

            foreach (AssetBundleInfo item in node.assetBundlesLoadData.BundlesToLoad) {
                num += item.Size;
                node.resourceLoadStat.BundleToProgress[item] = 0f;
            }

            node.resourceLoadStat.BytesTotal = num;
        }

        [OnEventFire]
        public void UpdateLoadProgress(UpdateEvent e, LoadingChannelNode loaderNode, [JoinBy(typeof(AssetGroupComponent))] LoadStatNode resource) {
            AssetBundleInfo info = loaderNode.assetBundleLoading.Info;
            ResourceLoadStatComponent resourceLoadStat = resource.resourceLoadStat;
            Dictionary<AssetBundleInfo, float> bundleToProgress = resourceLoadStat.BundleToProgress;

            if (!bundleToProgress.ContainsKey(info)) {
                resourceLoadStat.Progress = 1f;
                return;
            }

            float num = bundleToProgress[info];
            float num2 = bundleToProgress[info] = loaderNode.assetBundleLoading.AssetBundleDiskCacheRequest.Progress;

            if (!(Mathf.Abs(num2 - num) < 0.001f)) {
                float num3 = 0f;
                Dictionary<AssetBundleInfo, float>.Enumerator enumerator = bundleToProgress.GetEnumerator();

                while (enumerator.MoveNext()) {
                    KeyValuePair<AssetBundleInfo, float> current = enumerator.Current;
                    num3 += current.Key.Size * current.Value;
                }

                resourceLoadStat.Progress = resourceLoadStat.BytesTotal != 0 ? Mathf.Clamp01(num3 / resourceLoadStat.BytesTotal) : 1f;
                resourceLoadStat.BytesLoaded = Mathf.RoundToInt(resourceLoadStat.BytesTotal * resourceLoadStat.Progress);
            }
        }

        [OnEventFire]
        public void UpdateLoadProgress(AssetBundlesLoadedEvent e, SingleNode<ResourceLoadStatComponent> node) {
            ResourceLoadStatComponent component = node.component;
            component.BytesLoaded = component.BytesTotal;
            component.Progress = 1f;
        }

        public class LoadingChannelNode : Node {
            public AssetBundleLoadingComponent assetBundleLoading;
        }

        public class LoadStatNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;

            public ResourceLoadStatComponent resourceLoadStat;
        }
    }
}