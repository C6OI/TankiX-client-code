using System.IO;
using log4net.Core;

namespace log4net.Layout {
    public interface ILayout {
        string ContentType { get; }

        string Header { get; }

        string Footer { get; }

        bool IgnoresException { get; }

        void Format(TextWriter writer, LoggingEvent loggingEvent);
    }
}