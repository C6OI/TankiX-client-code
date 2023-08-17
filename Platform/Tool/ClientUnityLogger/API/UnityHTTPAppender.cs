using System.Collections.Generic;
using System.Text;
using log4net.Appender;
using log4net.Core;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.API {
    public class UnityHTTPAppender : AppenderSkeleton {
        readonly List<WWWLoader> wwwLoaders = new();
        int errorMessagesCounter;

        int messagesCounter;

        public UnityHTTPAppender() {
            url = "http://localhost";
            maxErrorMessages = 10;
            maxMessages = int.MaxValue;
            timeoutSeconds = 10;
        }

        public string url { get; set; }

        public int maxErrorMessages { get; set; }

        public int maxMessages { get; set; }

        public int timeoutSeconds { get; set; }

        protected override void Append(LoggingEvent loggingEvent) {
            RemoveExpiredLoaders();
            messagesCounter++;

            if (loggingEvent.Level >= Level.Error) {
                errorMessagesCounter++;
            }

            if (messagesCounter <= maxMessages && errorMessagesCounter <= maxErrorMessages) {
                string s = RenderLoggingEvent(loggingEvent);
                WWW www = new(url, Encoding.UTF8.GetBytes(s));
                AddLoader(www);
            }
        }

        void AddLoader(WWW www) {
            WWWLoader wWWLoader = new(www);
            wWWLoader.MaxRestartAttempts = 0;
            wWWLoader.TimeoutSeconds = timeoutSeconds;
            wwwLoaders.Add(wWWLoader);
        }

        void RemoveExpiredLoaders() {
            for (int i = 0; i < wwwLoaders.Count; i++) {
                WWWLoader wWWLoader = wwwLoaders[i];

                if (wWWLoader.IsDone) {
                    if (!string.IsNullOrEmpty(wWWLoader.Error)) {
                        Debug.LogWarningFormat("UnityHTTPAppender: {0}", wWWLoader.Error);
                    }

                    wWWLoader.Dispose();
                    wwwLoaders.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}