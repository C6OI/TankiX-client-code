using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class NdcPatternConverter : PatternLayoutConverter {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) =>
            WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty("NDC"));
    }
}