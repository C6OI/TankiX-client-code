using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class MethodLocationPatternConverter : PatternLayoutConverter {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) =>
            writer.Write(loggingEvent.LocationInformation.MethodName);
    }
}