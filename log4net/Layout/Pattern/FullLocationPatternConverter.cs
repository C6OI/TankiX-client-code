using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class FullLocationPatternConverter : PatternLayoutConverter {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) =>
            writer.Write(loggingEvent.LocationInformation.FullInfo);
    }
}