using System;
using System.Collections.Generic;
using log4net;
using Platform.Library.ClientLogger.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class WWWLoader : Loader, IDisposable {
        public static int DEFAULT_MAX_ATTEMPTS = 2;

        public static float DEFAULT_TIMEOUT_SECONDS = 30f;

        static readonly LinkedList<WWWLoader> activeLoaders = new();

        string errorMessage = string.Empty;

        float lastProgress;

        float lastProgressTime = Time.time;

        readonly ILog Log;

        public WWWLoader(WWW www) {
            WWW = www;
            Log = LoggerProvider.GetLogger(this);
            Log.InfoFormat("Loading {0}", www.url);
            activeLoaders.AddLast(this);
        }

        public int MaxRestartAttempts { get; set; } = DEFAULT_MAX_ATTEMPTS;

        public float TimeoutSeconds { get; set; } = DEFAULT_TIMEOUT_SECONDS;

        public WWW WWW { get; private set; }

        public byte[] Bytes => WWW.bytes;

        public float Progress => WWW.progress;

        public bool IsDone {
            get {
                if (WWW.isDone) {
                    if (!string.IsNullOrEmpty(WWW.error) && MaxRestartAttempts > 0) {
                        RestartLoad();
                        return false;
                    }

                    return true;
                }

                float time = Time.time;

                if (Math.Abs(WWW.progress - lastProgress) > float.Epsilon) {
                    lastProgress = WWW.progress;
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

        public string URL => WWW.url;

        public string Error => !string.IsNullOrEmpty(WWW.error) ? WWW.error : errorMessage;

        public void Dispose() {
            WWW.Dispose();
            activeLoaders.Remove(this);
        }

        public static void DisposeActiveLoaders() {
            while (activeLoaders.Count > 0) {
                activeLoaders.First.Value.Dispose();
            }
        }

        public void RestartLoad() {
            MaxRestartAttempts--;
            string url = WWW.url;
            Log.InfoFormat("RestartLoad URL: {0} restartAttempts: {1}", url, DEFAULT_MAX_ATTEMPTS - MaxRestartAttempts);
            WWW.Dispose();
            WWW = new WWW(url);
            lastProgress = 0f;
            lastProgressTime = Time.time;
            errorMessage = string.Empty;
        }

        public override string ToString() => string.Format("[WWWLoader URL={0}]", URL);

        public static int GetResponseCode(WWW request) {
            int result = 0;

            if (request.isDone && request.responseHeaders != null && request.responseHeaders.ContainsKey("STATUS")) {
                string[] array = request.responseHeaders["STATUS"].Split(' ');

                if (array.Length >= 3) {
                    int.TryParse(array[1], out result);
                }
            }

            return result;
        }
    }
}