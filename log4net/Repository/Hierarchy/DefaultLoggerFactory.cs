using log4net.Core;

namespace log4net.Repository.Hierarchy {
    class DefaultLoggerFactory : ILoggerFactory {
        public Logger CreateLogger(ILoggerRepository repository, string name) {
            if (name == null) {
                return new RootLogger(repository.LevelMap.LookupWithDefault(Level.Debug));
            }

            return new LoggerImpl(name);
        }

        internal sealed class LoggerImpl : Logger {
            internal LoggerImpl(string name)
                : base(name) { }
        }
    }
}