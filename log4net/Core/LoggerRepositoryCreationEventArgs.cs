using System;
using log4net.Repository;

namespace log4net.Core {
    public class LoggerRepositoryCreationEventArgs : EventArgs {
        public LoggerRepositoryCreationEventArgs(ILoggerRepository repository) => LoggerRepository = repository;

        public ILoggerRepository LoggerRepository { get; }
    }
}