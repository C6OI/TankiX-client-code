using System;
using System.Collections;
using System.IO;
using System.Text;

namespace log4net.DateFormatter {
    public class AbsoluteTimeDateFormatter : IDateFormatter {
        public const string AbsoluteTimeDateFormat = "ABSOLUTE";

        public const string DateAndTimeDateFormat = "DATE";

        public const string Iso8601TimeDateFormat = "ISO8601";

        static long s_lastTimeToTheSecond;

        static readonly StringBuilder s_lastTimeBuf = new();

        static readonly Hashtable s_lastTimeStrings = new();

        public virtual void FormatDate(DateTime dateToFormat, TextWriter writer) {
            lock (s_lastTimeStrings) {
                long num = dateToFormat.Ticks - dateToFormat.Ticks % 10000000;
                string text = null;

                if (s_lastTimeToTheSecond != num) {
                    s_lastTimeStrings.Clear();
                } else {
                    text = (string)s_lastTimeStrings[GetType()];
                }

                if (text == null) {
                    lock (s_lastTimeBuf) {
                        text = (string)s_lastTimeStrings[GetType()];

                        if (text == null) {
                            s_lastTimeBuf.Length = 0;
                            FormatDateWithoutMillis(dateToFormat, s_lastTimeBuf);
                            text = s_lastTimeBuf.ToString();
                            s_lastTimeStrings[GetType()] = text;
                            s_lastTimeToTheSecond = num;
                        }
                    }
                }

                writer.Write(text);
                writer.Write(',');
                int millisecond = dateToFormat.Millisecond;

                if (millisecond < 100) {
                    writer.Write('0');
                }

                if (millisecond < 10) {
                    writer.Write('0');
                }

                writer.Write(millisecond);
            }
        }

        protected virtual void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer) {
            int hour = dateToFormat.Hour;

            if (hour < 10) {
                buffer.Append('0');
            }

            buffer.Append(hour);
            buffer.Append(':');
            int minute = dateToFormat.Minute;

            if (minute < 10) {
                buffer.Append('0');
            }

            buffer.Append(minute);
            buffer.Append(':');
            int second = dateToFormat.Second;

            if (second < 10) {
                buffer.Append('0');
            }

            buffer.Append(second);
        }
    }
}