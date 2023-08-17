using System;
using System.IO;

namespace log4net.Util.PatternStringConverters {
    sealed class IdentityPatternConverter : PatternConverter {
        static readonly Type declaringType = typeof(IdentityPatternConverter);

        protected override void Convert(TextWriter writer, object state) => writer.Write(SystemInfo.NotAvailableText);
    }
}