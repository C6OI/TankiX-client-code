using System;
using System.Linq;
using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.API {
    public class UnityAppender : AppenderSkeleton {
        protected override void Append(LoggingEvent loggingEvent) {
            string message = RenderLoggingEvent(loggingEvent);
            Level actualLogLevel = GetActualLogLevel(loggingEvent);

            if (Level.Compare(actualLogLevel, Level.Fatal) >= 0) {
                Debug.LogException(new FatalException(message));
            } else if (Level.Compare(actualLogLevel, Level.Error) >= 0) {
                Debug.LogError(message);
            } else if (Level.Compare(actualLogLevel, Level.Warn) >= 0) {
                Debug.LogWarning(message);
            } else {
                Debug.Log(message);
            }
        }

        static Level GetActualLogLevel(LoggingEvent loggingEvent) => loggingEvent.Level;

        static bool IsRunningFromTest() {
            return AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith("NUnit"));
        }
    }
}