using System;

namespace log4net.Repository.Hierarchy {
    public class LoggerCreationEventArgs : EventArgs {
        public LoggerCreationEventArgs(Logger log) => Logger = log;

        public Logger Logger { get; }
    }
}