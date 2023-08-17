using log4net.Core;

namespace log4net.Layout {
    public class RawTimeStampLayout : IRawLayout {
        public virtual object Format(LoggingEvent loggingEvent) => loggingEvent.TimeStamp;
    }
}