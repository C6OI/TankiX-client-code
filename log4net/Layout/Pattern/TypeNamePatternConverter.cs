using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class TypeNamePatternConverter : NamedPatternConverter {
        protected override string GetFullyQualifiedName(LoggingEvent loggingEvent) =>
            loggingEvent.LocationInformation.ClassName;
    }
}