using System;
using System.IO;
using System.Xml;
using log4net.Core;
using log4net.Util;

namespace log4net.Layout {
    public abstract class XmlLayoutBase : LayoutSkeleton {
        protected XmlLayoutBase()
            : this(false) => IgnoresException = false;

        protected XmlLayoutBase(bool locationInfo) {
            IgnoresException = false;
            LocationInfo = locationInfo;
        }

        public bool LocationInfo { get; set; }

        public string InvalidCharReplacement { get; set; } = "?";

        public override string ContentType => "text/xml";

        public override void ActivateOptions() { }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            XmlTextWriter xmlTextWriter = new(new ProtectCloseTextWriter(writer));
            xmlTextWriter.Formatting = Formatting.None;
            xmlTextWriter.Namespaces = false;
            FormatXml(xmlTextWriter, loggingEvent);
            xmlTextWriter.WriteWhitespace(SystemInfo.NewLine);
            xmlTextWriter.Close();
        }

        protected abstract void FormatXml(XmlWriter writer, LoggingEvent loggingEvent);
    }
}