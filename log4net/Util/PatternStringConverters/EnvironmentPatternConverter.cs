using System;
using System.IO;
using System.Security;

namespace log4net.Util.PatternStringConverters {
    sealed class EnvironmentPatternConverter : PatternConverter {
        static readonly Type declaringType = typeof(EnvironmentPatternConverter);

        protected override void Convert(TextWriter writer, object state) {
            try {
                if (Option != null && Option.Length > 0) {
                    string environmentVariable = Environment.GetEnvironmentVariable(Option);

                    if (environmentVariable != null && environmentVariable.Length > 0) {
                        writer.Write(environmentVariable);
                    }
                }
            } catch (SecurityException exception) {
                LogLog.Debug(declaringType,
                    "Security exception while trying to expand environment variables. Error Ignored. No Expansion.",
                    exception);
            } catch (Exception exception2) {
                LogLog.Error(declaringType, "Error occurred while converting environment variable.", exception2);
            }
        }
    }
}