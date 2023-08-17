using log4net.Appender;
using Platform.Library.ClientLogger.Impl;

namespace Platform.Library.ClientLogger.API {
    public class ConsoleAppenderBuilder : AppenderBuilder {
        public ConsoleAppenderBuilder() => Init(new ConsoleAppender());
    }
}