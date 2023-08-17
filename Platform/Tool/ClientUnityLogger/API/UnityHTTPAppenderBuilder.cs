using Platform.Library.ClientLogger.Impl;

namespace Platform.Tool.ClientUnityLogger.API {
    public class UnityHTTPAppenderBuilder : AppenderBuilder {
        public UnityHTTPAppenderBuilder(string url) => Init(new UnityHTTPAppender {
            url = url
        });
    }
}