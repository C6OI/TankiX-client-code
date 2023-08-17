using System;
using System.Globalization;
using System.IO;

namespace log4net.DateFormatter {
    public class SimpleDateFormatter : IDateFormatter {
        readonly string m_formatString;

        public SimpleDateFormatter(string format) => m_formatString = format;

        public virtual void FormatDate(DateTime dateToFormat, TextWriter writer) =>
            writer.Write(dateToFormat.ToString(m_formatString, DateTimeFormatInfo.InvariantInfo));
    }
}