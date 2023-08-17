namespace log4net.Core {
    public abstract class LoggerWrapperImpl : ILoggerWrapper {
        protected LoggerWrapperImpl(ILogger logger) => Logger = logger;

        public virtual ILogger Logger { get; }
    }
}