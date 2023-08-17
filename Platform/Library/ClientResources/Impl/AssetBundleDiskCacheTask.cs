using System;
using System.Collections.Generic;
using log4net;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleDiskCacheTask : IDisposable {
        public static readonly int RELOAD_FROM_HTTP_ATTEMPTS = 2;

        public static readonly int CRC_RELOAD_ATTEMPS = 2;

        public static readonly int BUNDLE_RECRATION_ATTEMPS = 2;

        public static Crc32 CRC32 = new();

        readonly AssetBundleDiskCache assetBundleDiskCache;

        readonly Dictionary<AssetBundleDiskCacheState, Action> state2action = new(10);

        byte[] buffer;

        int bundleCreationAttempts = BUNDLE_RECRATION_ATTEMPS;

        int crcReloadAttempts = CRC_RELOAD_ATTEMPS;

        AssetBundleCreateRequest createRequest;

        int loadFromHttpAttempts = RELOAD_FROM_HTTP_ATTEMPTS;

        float taskNextRunTime;

        string url;

        WWW webRequest;

        DiskCacheWriterRequest writeRequest;

        public AssetBundleDiskCacheTask(AssetBundleDiskCache _assetBundleDiskCache) {
            assetBundleDiskCache = _assetBundleDiskCache;
            state2action.Add(AssetBundleDiskCacheState.INIT, Initialize);
            state2action.Add(AssetBundleDiskCacheState.START_LOAD_FROM_HTTP, StartLoadFromHTTP);
            state2action.Add(AssetBundleDiskCacheState.LOADING_FROM_HTTP, LoadingFromHTTP);
            state2action.Add(AssetBundleDiskCacheState.START_LOAD_FROM_DISK, StartLoadFromDisk);
            state2action.Add(AssetBundleDiskCacheState.START_WRITE_TO_DISK, StartWriteToDisk);
            state2action.Add(AssetBundleDiskCacheState.WRITE_TO_DISK, WriteToDisk);
            state2action.Add(AssetBundleDiskCacheState.CREATE_ASSETBUNDLE, CreateAssetBundle);
            state2action.Add(AssetBundleDiskCacheState.COMPLETE, Complete);
            Log = LoggerProvider.GetLogger(this);
        }

        public AssetBundleDiskCacheRequest AssetBundleDiskCacheRequest { get; private set; }

        public bool IsDone { get; private set; }

        public AssetBundleInfo AssetBundleInfo { get; private set; }

        public AssetBundle AssetBundle { get; private set; }

        public string Error { get; private set; }

        public float Progress { get; private set; }

        public AssetBundleDiskCacheState State { get; private set; }

        ILog Log { get; set; }

        public void Dispose() => buffer = null;

        public AssetBundleDiskCacheTask Init(AssetBundleDiskCacheRequest request) {
            AssetBundleInfo = request.AssetBundleInfo;
            AssetBundleDiskCacheRequest = request;
            return this;
        }

        public bool Run() {
            AssetBundleDiskCacheState state;

            do {
                if (Time.realtimeSinceStartup < taskNextRunTime) {
                    return IsDone;
                }

                state = State;
                state2action[State]();
            } while (state != State);

            UpdateRequest();
            return IsDone;
        }

        void Initialize() {
            if (assetBundleDiskCache.IsCached(AssetBundleInfo)) {
                State = AssetBundleDiskCacheState.START_LOAD_FROM_DISK;
            } else {
                State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
            }
        }

        public void Complete() {
            Progress = 1f;
            IsDone = true;
        }

        void StartLoadFromHTTP() {
            url = AssetBundleNaming.GetAssetBundleUrl(assetBundleDiskCache.BaseUrl, AssetBundleInfo.Filename);

            if (loadFromHttpAttempts < RELOAD_FROM_HTTP_ATTEMPTS) {
                url = string.Format("{0}?rnd={1}", url, Random.value);
            }

            Console.WriteLine("Start download url: {0}", url);
            webRequest = new WWW(url);
            State = AssetBundleDiskCacheState.LOADING_FROM_HTTP;
        }

        void LoadingFromHTTP() {
            if (!string.IsNullOrEmpty(webRequest.error)) {
                if (loadFromHttpAttempts-- > 0) {
                    Console.WriteLine("{0}, url: {1}, try to download it agan ", webRequest.error, url);
                    webRequest.Dispose();
                    State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
                } else {
                    Error = string.Format("{0}, url: {1}", webRequest.error, url);
                    State = AssetBundleDiskCacheState.COMPLETE;
                }

                return;
            }

            Progress = webRequest.progress;

            if (webRequest.isDone) {
                buffer = webRequest.bytes;

                if (!AssetBundleDiskCacheRequest.UseCrcCheck || CheckCrc()) {
                    webRequest.Dispose();
                    State = AssetBundleDiskCacheState.START_WRITE_TO_DISK;
                }
            }
        }

        bool CheckCrc() {
            if (CRC32.Compute(buffer) != AssetBundleInfo.CacheCRC) {
                if (crcReloadAttempts-- > 0) {
                    Console.WriteLine("Crc mismatch while loading {0}, try to download it agan ",
                        AssetBundleInfo.BundleName);

                    buffer = null;
                    State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
                    return false;
                }

                Error = string.Format("Crc mismatch while loading {0}", AssetBundleInfo.BundleName);
                State = AssetBundleDiskCacheState.COMPLETE;
                return false;
            }

            return true;
        }

        void StartWriteToDisk() {
            writeRequest =
                assetBundleDiskCache.WriteToDiskCache(assetBundleDiskCache.GetAssetBundleCachePath(AssetBundleInfo), buffer);

            State = AssetBundleDiskCacheState.WRITE_TO_DISK;
        }

        void WriteToDisk() {
            if (writeRequest.IsDone) {
                if (!string.IsNullOrEmpty(writeRequest.Error)) {
                    Error = writeRequest.Error;
                    State = AssetBundleDiskCacheState.COMPLETE;
                } else {
                    State = AssetBundleDiskCacheState.START_LOAD_FROM_DISK;
                }
            }
        }

        void StartLoadFromDisk() {
            string assetBundleCachePath = assetBundleDiskCache.GetAssetBundleCachePath(AssetBundleInfo);
            createRequest = AssetBundle.LoadFromFileAsync(assetBundleCachePath);
            State = AssetBundleDiskCacheState.CREATE_ASSETBUNDLE;
        }

        void CreateAssetBundle() {
            if (!createRequest.isDone) {
                return;
            }

            AssetBundle = createRequest.assetBundle;

            if (AssetBundle == null) {
                if (HandleRestartOnBundleCreationFail()) {
                    return;
                }

                Error = string.Format("failed to create assetbundle {0}", AssetBundleInfo.BundleName);
            }

            State = AssetBundleDiskCacheState.COMPLETE;
        }

        bool HandleRestartOnBundleCreationFail() {
            if (bundleCreationAttempts-- > 0) {
                Sleep(0.5f);
                Console.WriteLine("Failed to create assetBundle {0}, try to create it agan ", AssetBundleInfo.BundleName);
                State = AssetBundleDiskCacheState.START_LOAD_FROM_DISK;
                return true;
            }

            if (loadFromHttpAttempts-- > 0) {
                Console.WriteLine("Failed to create assetBundle {0}, try to download it agan ", AssetBundleInfo.BundleName);

                if (assetBundleDiskCache.CleanCache(AssetBundleInfo)) {
                    State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
                    return true;
                }
            }

            return false;
        }

        void UpdateRequest() {
            AssetBundleDiskCacheRequest.IsDone = IsDone;
            AssetBundleDiskCacheRequest.AssetBundle = AssetBundle;
            AssetBundleDiskCacheRequest.Progress = Progress;
            AssetBundleDiskCacheRequest.Error = Error;
        }

        public void Sleep(float seconds) => taskNextRunTime = Time.realtimeSinceStartup + seconds;

        struct TaskRunner {
            public Action runner;

            public Action error;

            public Action timeOut;

            public float timeOutValue;
        }
    }
}