using System;
using System.Globalization;
using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class RelativeTimePatternConverter : PatternLayoutConverter {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) => writer.Write(
            TimeDifferenceInMillis(LoggingEvent.StartTime, loggingEvent.TimeStamp).ToString(NumberFormatInfo.InvariantInfo));

        static long TimeDifferenceInMillis(DateTime start, DateTime end) =>
            (long)(end.ToUniversalTime() - start.ToUniversalTime()).TotalMilliseconds;
    }
}