using System.Globalization;
using System.IO;
using log4net.Core;

namespace log4net.Layout {
    public abstract class LayoutSkeleton : IOptionHandler, ILayout {
        public virtual string ContentType => "text/plain";

        public virtual string Header { get; set; }

        public virtual string Footer { get; set; }

        public virtual bool IgnoresException { get; set; } = true;

        public abstract void Format(TextWriter writer, LoggingEvent loggingEvent);

        public abstract void ActivateOptions();

        public string Format(LoggingEvent loggingEvent) {
            StringWriter stringWriter = new(CultureInfo.InvariantCulture);
            Format(stringWriter, loggingEvent);
            return stringWriter.ToString();
        }
    }
}