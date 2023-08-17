using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class MessagePatternConverter : PatternLayoutConverter {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) =>
            loggingEvent.WriteRenderedMessage(writer);
    }
}