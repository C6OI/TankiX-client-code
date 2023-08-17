using System;
using System.IO;
using log4net.Core;
using log4net.Util;

namespace log4net.Layout.Pattern {
    public abstract class NamedPatternConverter : PatternLayoutConverter, IOptionHandler {
        const string DOT = ".";

        static readonly Type declaringType = typeof(NamedPatternConverter);

        int m_precision;

        public void ActivateOptions() {
            m_precision = 0;

            if (Option == null) {
                return;
            }

            string text = Option.Trim();

            if (text.Length <= 0) {
                return;
            }

            int val;

            if (SystemInfo.TryParse(text, out val)) {
                if (val <= 0) {
                    LogLog.Error(declaringType,
                        "NamedPatternConverter: Precision option (" + text + ") isn't a positive integer.");
                } else {
                    m_precision = val;
                }
            } else {
                LogLog.Error(declaringType,
                    "NamedPatternConverter: Precision option \"" + text + "\" not a decimal integer.");
            }
        }

        protected abstract string GetFullyQualifiedName(LoggingEvent loggingEvent);

        protected sealed override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            string text = GetFullyQualifiedName(loggingEvent);

            if (m_precision <= 0 || text == null || text.Length < 2) {
                writer.Write(text);
                return;
            }

            int num = text.Length;
            string text2 = string.Empty;

            if (text.EndsWith(".")) {
                text2 = ".";
                text = text.Substring(0, num - 1);
                num--;
            }

            int num2 = text.LastIndexOf(".");
            int num3 = 1;

            while (num2 > 0 && num3 < m_precision) {
                num2 = text.LastIndexOf('.', num2 - 1);
                num3++;
            }

            if (num2 == -1) {
                writer.Write(text + text2);
            } else {
                writer.Write(text.Substring(num2 + 1, num - num2 - 1) + text2);
            }
        }
    }
}