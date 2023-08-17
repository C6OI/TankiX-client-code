using System;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace log4net.Util.PatternStringConverters {
    sealed class ProcessIdPatternConverter : PatternConverter {
        static readonly Type declaringType = typeof(ProcessIdPatternConverter);

        protected override void Convert(TextWriter writer, object state) {
            try {
                writer.Write(Process.GetCurrentProcess().Id);
            } catch (SecurityException) {
                LogLog.Debug(declaringType, "Security exception while trying to get current process id. Error Ignored.");
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}