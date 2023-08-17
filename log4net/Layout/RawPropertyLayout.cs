using log4net.Core;

namespace log4net.Layout {
    public class RawPropertyLayout : IRawLayout {
        public string Key { get; set; }

        public virtual object Format(LoggingEvent loggingEvent) => loggingEvent.LookupProperty(Key);
    }
}