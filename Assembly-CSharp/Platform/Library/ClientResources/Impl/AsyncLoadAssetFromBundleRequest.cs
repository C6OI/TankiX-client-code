using System;
using System.Collections.Generic;
using log4net;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Platform.Library.ClientResources.Impl {
    public class AsyncLoadAssetFromBundleRequest : LoadAssetFromBundleRequest {
        static ILog log;

        static readonly List<AssetBundleRequest> assetBundleRequestList = new();

        AssetBundleRequest assetBundleRequest;

        bool isStreamedSceneAssetBundle;

        public AsyncLoadAssetFromBundleRequest(AssetBundle bundle, string objectName, Type objectType) {
            Bundle = bundle;
            ObjectName = objectName;
            ObjectType = objectType;
        }

        public AssetBundle Bundle { get; }

        public string ObjectName { get; }

        public Type ObjectType { get; }

        public bool IsStarted { get; private set; }

        public bool IsDone {
            get {
                if (!IsStarted) {
                    return false;
                }

                if (isStreamedSceneAssetBundle) {
                    return true;
                }

                if (assetBundleRequest == null) {
                    return false;
                }

                return assetBundleRequest.isDone;
            }
        }

        public Object Asset {
            get {
                if (isStreamedSceneAssetBundle) {
                    return null;
                }

                if (!IsDone) {
                    return null;
                }

                assetBundleRequestList.Remove(assetBundleRequest);
                return assetBundleRequest.asset;
            }
        }

        public void StartExecute() {
            IsStarted = true;
            GetLogger().InfoFormat("LoadAssetAsync objectName={0} objectType={1}", ObjectName, ObjectType);

            if (Bundle.isStreamedSceneAssetBundle) {
                isStreamedSceneAssetBundle = true;
                return;
            }

            assetBundleRequest = Bundle.LoadAssetAsync(ObjectName, ObjectType);
            assetBundleRequestList.Add(assetBundleRequest);
        }

        ILog GetLogger() {
            if (log == null) {
                log = LoggerProvider.GetLogger(this);
            }

            return log;
        }
    }
}