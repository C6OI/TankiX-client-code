using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class PropertyPatternConverter : PatternLayoutConverter {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            if (Option != null) {
                WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty(Option));
            } else {
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }
    }
}