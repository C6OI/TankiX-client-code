using System;
using log4net;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class WWWLoader : IDisposable, Loader {
        public static int DEFAULT_MAX_ATTEMPTS = 2;

        public static float DEFAULT_TIMEOUT_SECONDS = 30f;

        readonly ILog Log;

        string errorMessage = string.Empty;

        float lastProgress;

        float lastProgressTime = UnityTime.time;

        WWW www;

        public WWWLoader(WWW www) {
            this.www = www;
            Log = LoggerProvider.GetLogger(this);
            Log.InfoFormat("Loading {0}", www.url);
        }

        [Inject] public static UnityTime UnityTime { get; set; }

        public int MaxRestartAttempts { get; set; } = DEFAULT_MAX_ATTEMPTS;

        public float TimeoutSeconds { get; set; } = DEFAULT_TIMEOUT_SECONDS;

        public void Dispose() => www.Dispose();

        public AssetBundle AssetBundle => www.assetBundle;

        public byte[] Bytes => www.bytes;

        public float Progress => www.progress;

        public bool IsDone {
            get {
                if (www.isDone) {
                    if (!string.IsNullOrEmpty(www.error) && MaxRestartAttempts > 0) {
                        RestartLoad();
                        return false;
                    }

                    return true;
                }

                float time = UnityTime.time;

                if (Math.Abs(www.progress - lastProgress) > float.Epsilon) {
                    lastProgress = www.progress;
                    lastProgressTime = time;
                    return false;
                }

                if (time - lastProgressTime > TimeoutSeconds) {
                    if (MaxRestartAttempts > 0) {
                        RestartLoad();
                        return false;
                    }

                    Log.InfoFormat("Fail URL: {0}", URL);
                    errorMessage = "Pause of loading was too long";
                    return true;
                }

                return false;
            }
        }

        public string URL => www.url;

        public string Error => !string.IsNullOrEmpty(www.error) ? www.error : errorMessage;

        public void RestartLoad() {
            MaxRestartAttempts--;
            string url = www.url;
            Log.InfoFormat("RestartLoad URL: {0} restartAttempts: {1}", url, DEFAULT_MAX_ATTEMPTS - MaxRestartAttempts);
            www.Dispose();
            www = new WWW(url);
            lastProgress = 0f;
            lastProgressTime = UnityTime.time;
            errorMessage = string.Empty;
        }

        public override string ToString() => string.Format("[WWWLoader URL={0}]", URL);
    }
}