using Platform.Library.ClientLogger.Impl;

namespace Platform.Tool.ClientUnityLogger.API {
    public class UnityAppenderBuilder : AppenderBuilder {
        public UnityAppenderBuilder() {
            Init(new UnityAppender());
        }
    }
}