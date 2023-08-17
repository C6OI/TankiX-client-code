using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class LoggerPatternConverter : NamedPatternConverter {
        protected override string GetFullyQualifiedName(LoggingEvent loggingEvent) => loggingEvent.LoggerName;
    }
}