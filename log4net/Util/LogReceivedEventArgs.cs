using System;

namespace log4net.Util {
    public class LogReceivedEventArgs : EventArgs {
        public LogReceivedEventArgs(LogLog loglog) => LogLog = loglog;

        public LogLog LogLog { get; }
    }
}