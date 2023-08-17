using System;
using System.IO;
using log4net.Core;
using log4net.Util;

namespace log4net.Layout.Pattern {
    class StackTracePatternConverter : PatternLayoutConverter, IOptionHandler {
        static readonly Type declaringType = typeof(StackTracePatternConverter);
        int m_stackFrameLevel = 1;

        public void ActivateOptions() {
            if (Option == null) {
                return;
            }

            string text = Option.Trim();

            if (text.Length == 0) {
                return;
            }

            int val;

            if (SystemInfo.TryParse(text, out val)) {
                if (val <= 0) {
                    LogLog.Error(declaringType,
                        "StackTracePatternConverter: StackeFrameLevel option (" + text + ") isn't a positive integer.");
                } else {
                    m_stackFrameLevel = val;
                }
            } else {
                LogLog.Error(declaringType,
                    "StackTracePatternConverter: StackFrameLevel option \"" + text + "\" not a decimal integer.");
            }
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            StackFrameItem[] stackFrames = loggingEvent.LocationInformation.StackFrames;

            if (stackFrames == null || stackFrames.Length <= 0) {
                LogLog.Error(declaringType, "loggingEvent.LocationInformation.StackFrames was null or empty.");
                return;
            }

            int num = m_stackFrameLevel - 1;

            while (num >= 0) {
                if (num >= stackFrames.Length) {
                    num--;
                    continue;
                }

                StackFrameItem stackFrameItem = stackFrames[num];
                writer.Write("{0}.{1}", stackFrameItem.ClassName, GetMethodInformation(stackFrameItem.Method));

                if (num > 0) {
                    writer.Write(" > ");
                }

                num--;
            }
        }

        internal virtual string GetMethodInformation(MethodItem method) => method.Name;
    }
}