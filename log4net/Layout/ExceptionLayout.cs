using System;
using System.IO;
using log4net.Core;

namespace log4net.Layout {
    public class ExceptionLayout : LayoutSkeleton {
        public ExceptionLayout() => IgnoresException = false;

        public override void ActivateOptions() { }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            writer.Write(loggingEvent.GetExceptionString());
        }
    }
}