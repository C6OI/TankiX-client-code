using System.IO;

namespace log4net.Util.PatternStringConverters {
    sealed class AppDomainPatternConverter : PatternConverter {
        protected override void Convert(TextWriter writer, object state) => writer.Write(SystemInfo.ApplicationFriendlyName);
    }
}