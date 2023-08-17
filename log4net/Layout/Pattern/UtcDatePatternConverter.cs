using System;
using System.IO;
using log4net.Core;
using log4net.Util;

namespace log4net.Layout.Pattern {
    class UtcDatePatternConverter : DatePatternConverter {
        static readonly Type declaringType = typeof(UtcDatePatternConverter);

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            try {
                m_dateFormatter.FormatDate(loggingEvent.TimeStamp.ToUniversalTime(), writer);
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Error occurred while converting date.", exception);
            }
        }
    }
}