using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleLoadSystem : ECSSystem {
        const int MAX_LOADING_CHANNELS_COUNT = 4;

        readonly PriorityAssetBundleNodeComparer loadPriorityComparer = new();

        [OnEventFire]
        public void InitSystem(NodeAddedEvent e, AssetBundleDatabaseNode database) {
            AssetBundleDatabase assetBundleDatabase = database.assetBundleDatabase.AssetBundleDatabase;
            List<AssetBundleInfo> allAssetBundles = assetBundleDatabase.GetAllAssetBundles();

            for (int i = 0; i < allAssetBundles.Count; i++) {
                AssetBundleInfo assetBundleInfo = allAssetBundles[i];
                assetBundleInfo.IsCached = database.assetBundleDiskCache.AssetBundleDiskCache.IsCached(assetBundleInfo);
            }

            CreateLoadingChannels(database.Entity);
        }

        void CreateLoadingChannels(Entity assetBundleDatabaseEntity) {
            AssetBundleLoadingChannelsCountComponent assetBundleLoadingChannelsCountComponent = new();

            int num3 = assetBundleLoadingChannelsCountComponent.ChannelsCount =
                           assetBundleLoadingChannelsCountComponent.DefaultChannelsCount = Math.Max(1, Math.Min(SystemInfo.processorCount - 1, 4));

            assetBundleDatabaseEntity.AddComponent(assetBundleLoadingChannelsCountComponent);

            for (int i = 0; i < num3; i++) {
                CreateAssetBundleLoadingChannel();
            }
        }

        void CreateAssetBundleLoadingChannel() {
            Entity entity = CreateEntity("LoadingChannel");
            entity.AddComponent<LoadingChannelComponent>();
            entity.AddComponent<LoadingChannelIdleComponent>();
        }

        [OnEventFire]
        public void UpdateLoadingChannelsCount(UpdateEvent e, SingleNode<AssetBundleLoadingChannelsCountComponent> channelsCount,
            [JoinAll] ICollection<SingleNode<LoadingChannelComponent>> loadingChannels) {
            int channelsCount2 = channelsCount.component.ChannelsCount;
            int num = loadingChannels.Count;

            if (channelsCount2 == num) {
                return;
            }

            if (channelsCount2 > num) {
                for (int i = 0; i < channelsCount2 - num; i++) {
                    CreateAssetBundleLoadingChannel();
                }

                return;
            }

            foreach (SingleNode<LoadingChannelComponent> loadingChannel in loadingChannels) {
                if (loadingChannel.Entity.HasComponent<LoadingChannelIdleComponent>()) {
                    DeleteEntity(loadingChannel.Entity);
                    num--;

                    if (channelsCount2 == num) {
                        break;
                    }
                }
            }
        }

        [OnEventFire]
        public void PrepareBundlesRequest(NodeAddedEvent e, LoadBundlesForAssetRequestNode request, [JoinAll] SingleNode<AssetBundleDatabaseComponent> assetBundleDatabase) {
            AssetInfo assetInfo = assetBundleDatabase.component.AssetBundleDatabase.GetAssetInfo(request.assetReference.Reference.AssetGuid);
            HashSet<AssetBundleInfo> hashSet = new();
            CollectBundles(assetInfo, hashSet);
            PrepareAssetBundlesRequest(request.Entity, hashSet);
        }

        [OnEventFire]
        public void CancelBundlesRequest(NodeRemoveEvent e, SingleNode<LoadAssetBundlesRequestComponent> request) {
            Entity entity = request.Entity;
            CleanBundleRequest(entity);
        }

        [OnEventFire]
        public void UpdateBundlesRequests(UpdateEvent e, SingleNode<BaseUrlComponent> baseUrlNode, [JoinAll] ICollection<BundlesRequestNode> bundlesRequestList) {
            UpdateBundleRequests(bundlesRequestList);
        }

        void UpdateBundleRequests(ICollection<BundlesRequestNode> bundlesRequestList) {
            if (bundlesRequestList.Count != 0) {
                List<BundlesRequestNode> list = Sort(bundlesRequestList);

                for (int num = list.Count - 1; num >= 0; num--) {
                    BundlesRequestNode bundlesRequest = list[num];
                    UpdateAssetBundleRequest(bundlesRequest);
                }
            }
        }

        void UpdateAssetBundleRequest(BundlesRequestNode bundlesRequest) {
            if (!TryCompleteLoading(bundlesRequest)) {
                ScheduleEvent<PrepareBundleToLoadEvent>(bundlesRequest);
            }
        }

        [OnEventFire]
        [Mandatory]
        public void PrepareBundleToLoad(PrepareBundleToLoadEvent e, BundlesRequestNode bundlesRequest, [JoinAll] ICollection<IdleLoadingChannelNode> loadingChannels,
            [JoinAll] SingleNode<BaseUrlComponent> baseUrlNode) {
            foreach (IdleLoadingChannelNode loadingChannel in loadingChannels) {
                AssetBundleInfo assetBundleInfo = SelectDependBundleToLoad(bundlesRequest);

                if (assetBundleInfo != null) {
                    PrepareLoadingChannel(assetBundleInfo, baseUrlNode.component.Url, loadingChannel, bundlesRequest.Entity);
                    continue;
                }

                break;
            }
        }

        void PrepareLoadingChannel(AssetBundleInfo info, string baseUrl, IdleLoadingChannelNode channelNode, Entity ownerBundle) {
            AssetBundlesStorage.MarkLoading(info);
            Entity entity = channelNode.Entity;
            entity.RemoveComponent<LoadingChannelIdleComponent>();
            AssetBundleLoadingComponent component = new(info);
            entity.AddComponent(component);
            entity.AddComponent(new AssetGroupComponent(ownerBundle));
        }

        [OnEventComplete]
        public void HandleBundleLoadComplete(LoadCompleteEvent e, AssetBundleLoadingNode node, [JoinAll] AssetBundleDatabaseNode dbNode) {
            AssetBundleLoadingComponent assetBundleLoading = node.assetBundleLoading;
            AssetBundleDiskCache assetBundleDiskCache = dbNode.assetBundleDiskCache.AssetBundleDiskCache;
            AssetBundleInfo info = node.assetBundleLoading.Info;

            if (!info.IsCached) {
                LogDownloadInfoIfBundleIsBig(assetBundleLoading, AssetBundleNaming.GetAssetBundleUrl(assetBundleDiskCache.BaseUrl, info.Filename), info.Size);
                info.IsCached = true;
            }

            ReleaseLoadingChannel(node.Entity);
        }

        void ReleaseLoadingChannel(Entity channelEntity) {
            channelEntity.RemoveComponent<AssetBundleLoadingComponent>();
            channelEntity.RemoveComponent<AssetGroupComponent>();
            channelEntity.AddComponent<LoadingChannelIdleComponent>();
        }

        void LogDownloadInfoIfBundleIsBig(AssetBundleLoadingComponent assetBundleLoadingComponent, string url, int size) {
            if (size > 15000000) {
                float num = Time.realtimeSinceStartup - assetBundleLoadingComponent.StartTime;
                Log.InfoFormat("AssetBundleDownloadInfo\n url: {0}\n loadingTimeInSec: {1}\n bytesDownloaded: {2}", url, num, size);
            }
        }

        List<BundlesRequestNode> Sort(ICollection<BundlesRequestNode> requestedBundles) {
            List<BundlesRequestNode> list = new(requestedBundles);
            list.Sort(loadPriorityComparer);
            return list;
        }

        AssetBundleInfo SelectDependBundleToLoad(BundlesRequestNode bundlesRequest) {
            AssetBundlesLoadDataComponent assetBundlesLoadData = bundlesRequest.assetBundlesLoadData;
            AssetBundleInfo result = null;
            List<AssetBundleInfo> list = new();

            for (int i = 0; i < assetBundlesLoadData.BundlesToLoad.Count; i++) {
                AssetBundleInfo assetBundleInfo = assetBundlesLoadData.BundlesToLoad[i];

                if (AssetBundlesStorage.IsStored(assetBundleInfo)) {
                    list.Add(assetBundleInfo);
                } else if (!AssetBundlesStorage.IsLoading(assetBundleInfo)) {
                    if (i < assetBundlesLoadData.BundlesToLoad.Count - 1) {
                        assetBundlesLoadData.BundlesToLoad.Remove(assetBundleInfo);
                        assetBundlesLoadData.LoadingBundles.Add(assetBundleInfo);
                        result = assetBundleInfo;
                    } else if (assetBundlesLoadData.BundlesToLoad.Count == 1 && assetBundlesLoadData.LoadingBundles.Count == 0) {
                        assetBundlesLoadData.BundlesToLoad.Remove(assetBundleInfo);
                        assetBundlesLoadData.LoadingBundles.Add(assetBundleInfo);
                        result = assetBundleInfo;
                    }

                    break;
                }
            }

            foreach (AssetBundleInfo item in list) {
                assetBundlesLoadData.BundlesToLoad.Remove(item);
                Dictionary<AssetBundleInfo, AssetBundle> loadedBundles = assetBundlesLoadData.LoadedBundles;

                if (!loadedBundles.ContainsKey(item)) {
                    loadedBundles.Add(item, AssetBundlesStorage.Get(item));
                }
            }

            return result;
        }

        [OnEventFire]
        public void MarkLoaded(LoadCompleteEvent e, PreparedLoaderNode node, [JoinBy(typeof(AssetGroupComponent))] BundlesRequestNode bundlesRequestNode) {
            string bundleName = node.assetBundleLoading.Info.BundleName;
            AssetBundle assetBundle = node.assetBundleLoading.AssetBundleDiskCacheRequest.AssetBundle;
            bundlesRequestNode.assetBundlesLoadData.LoadingBundles.Remove(node.assetBundleLoading.Info);
            bundlesRequestNode.assetBundlesLoadData.LoadedBundles.Add(node.assetBundleLoading.Info, assetBundle);
        }

        bool TryCompleteLoading(BundlesRequestNode bundlesRequest) {
            if (bundlesRequest.assetBundlesLoadData.Loaded) {
                return true;
            }

            if (IsLoadingComplete(bundlesRequest)) {
                bundlesRequest.assetBundlesLoadData.Loaded = true;
                ScheduleEvent<AssetBundlesLoadedEvent>(bundlesRequest);
                return true;
            }

            return false;
        }

        bool IsLoadingComplete(BundlesRequestNode bundle) => bundle.assetBundlesLoadData.BundlesToLoad.Count == 0 && bundle.assetBundlesLoadData.LoadingBundles.Count == 0;

        void CollectBundles(AssetInfo info, ICollection<AssetBundleInfo> dependencies) {
            foreach (AssetBundleInfo allDependency in info.AssetBundleInfo.AllDependencies) {
                dependencies.Add(allDependency);
            }

            dependencies.Add(info.AssetBundleInfo);
        }

        void PrepareAssetBundlesRequest(Entity request, HashSet<AssetBundleInfo> bundleInfos) {
            request.AddComponent(new AssetGroupComponent(request));
            request.AddComponent<ResourceLoadStatComponent>();
            AssetBundlesLoadDataComponent assetBundlesLoadDataComponent = new();
            assetBundlesLoadDataComponent.AllBundles = bundleInfos;
            assetBundlesLoadDataComponent.BundlesToLoad = new List<AssetBundleInfo>(bundleInfos);
            assetBundlesLoadDataComponent.LoadingBundles = new HashSet<AssetBundleInfo>();
            assetBundlesLoadDataComponent.LoadedBundles = new Dictionary<AssetBundleInfo, AssetBundle>();
            request.AddComponent(assetBundlesLoadDataComponent);
        }

        static void CleanBundleRequest(Entity entity) {
            if (entity.HasComponent<AssetGroupComponent>()) {
                entity.RemoveComponent<AssetGroupComponent>();
            }

            if (entity.HasComponent<ResourceLoadStatComponent>()) {
                entity.RemoveComponent<ResourceLoadStatComponent>();
            }

            if (entity.HasComponent<AssetBundlesLoadDataComponent>()) {
                entity.RemoveComponent<AssetBundlesLoadDataComponent>();
            }
        }

        public class BundlesRequestNode : Node {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetGroupComponent assetGroup;

            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class LoadBundlesForAssetRequestNode : Node {
            public AssetReferenceComponent assetReference;

            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class IdleLoadingChannelNode : Node {
            public LoadingChannelComponent loadingChannel;

            public LoadingChannelIdleComponent loadingChannelIdle;
        }

        public class PreparedLoaderNode : Node {
            public AssetBundleLoadingComponent assetBundleLoading;

            public AssetGroupComponent assetGroup;
        }

        public class AssetBundleLoadingNode : Node {
            public AssetBundleLoadingComponent assetBundleLoading;
        }

        public class AssetBundleDatabaseNode : Node {
            public AssetBundleDatabaseComponent assetBundleDatabase;

            public AssetBundleDiskCacheComponent assetBundleDiskCache;

            public BaseUrlComponent baseUrl;
        }

        class PriorityAssetBundleNodeComparer : IComparer<BundlesRequestNode> {
            public int Compare(BundlesRequestNode x, BundlesRequestNode y) =>
                Comparer<int>.Default.Compare(x.loadAssetBundlesRequest.LoadingPriority, y.loadAssetBundlesRequest.LoadingPriority);
        }
    }
}