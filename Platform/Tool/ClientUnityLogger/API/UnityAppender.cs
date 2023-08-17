using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.API {
    public class UnityAppender : AppenderSkeleton {
        protected override void Append(LoggingEvent loggingEvent) {
            string message = RenderLoggingEvent(loggingEvent);

            if (Level.Compare(loggingEvent.Level, Level.Fatal) >= 0) {
                Debug.LogException(new FatalException(message));
            } else if (Level.Compare(loggingEvent.Level, Level.Error) >= 0) {
                Debug.LogError(message);
            } else if (Level.Compare(loggingEvent.Level, Level.Warn) >= 0) {
                Debug.LogWarning(message);
            } else {
                Debug.Log(message);
            }
        }
    }
}