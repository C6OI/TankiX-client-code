using System;
using System.IO;
using System.Security;

namespace log4net.Util.PatternStringConverters {
    sealed class EnvironmentFolderPathPatternConverter : PatternConverter {
        static readonly Type declaringType = typeof(EnvironmentFolderPathPatternConverter);

        protected override void Convert(TextWriter writer, object state) {
            try {
                if (Option != null && Option.Length > 0) {
                    Environment.SpecialFolder folder =
                        (Environment.SpecialFolder)(int)Enum.Parse(typeof(Environment.SpecialFolder), Option, true);

                    string folderPath = Environment.GetFolderPath(folder);

                    if (folderPath != null && folderPath.Length > 0) {
                        writer.Write(folderPath);
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