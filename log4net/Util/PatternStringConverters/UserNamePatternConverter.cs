using System;
using System.IO;

namespace log4net.Util.PatternStringConverters {
    sealed class UserNamePatternConverter : PatternConverter {
        static readonly Type declaringType = typeof(UserNamePatternConverter);

        protected override void Convert(TextWriter writer, object state) => writer.Write(SystemInfo.NotAvailableText);
    }
}