using System;
using System.Globalization;
using System.IO;
using log4net.Core;
using log4net.DateFormatter;
using log4net.Util;

namespace log4net.Layout.Pattern {
    class DatePatternConverter : PatternLayoutConverter, IOptionHandler {
        static readonly Type declaringType = typeof(DatePatternConverter);
        protected IDateFormatter m_dateFormatter;

        public void ActivateOptions() {
            string text = Option;

            if (text == null) {
                text = "ISO8601";
            }

            if (string.Compare(text, "ISO8601", true, CultureInfo.InvariantCulture) == 0) {
                m_dateFormatter = new Iso8601DateFormatter();
                return;
            }

            if (string.Compare(text, "ABSOLUTE", true, CultureInfo.InvariantCulture) == 0) {
                m_dateFormatter = new AbsoluteTimeDateFormatter();
                return;
            }

            if (string.Compare(text, "DATE", true, CultureInfo.InvariantCulture) == 0) {
                m_dateFormatter = new DateTimeDateFormatter();
                return;
            }

            try {
                m_dateFormatter = new SimpleDateFormatter(text);
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Could not instantiate SimpleDateFormatter with [" + text + "]", exception);
                m_dateFormatter = new Iso8601DateFormatter();
            }
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            try {
                m_dateFormatter.FormatDate(loggingEvent.TimeStamp, writer);
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Error occurred while converting date.", exception);
            }
        }
    }
}