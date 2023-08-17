using System.IO;
using log4net.Core;

namespace log4net.Layout.Pattern {
    sealed class ExceptionPatternConverter : PatternLayoutConverter {
        public ExceptionPatternConverter() => IgnoresException = false;

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            if (loggingEvent.ExceptionObject != null && Option != null && Option.Length > 0) {
                switch (Option.ToLower()) {
                    case "message":
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.Message);
                        break;

                    case "source":
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.Source);
                        break;

                    case "stacktrace":
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.StackTrace);
                        break;

                    case "targetsite":
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.TargetSite);
                        break;

                    case "helplink":
                        WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.HelpLink);
                        break;
                }
            } else {
                string exceptionString = loggingEvent.GetExceptionString();

                if (exceptionString != null && exceptionString.Length > 0) {
                    writer.WriteLine(exceptionString);
                }
            }
        }
    }
}