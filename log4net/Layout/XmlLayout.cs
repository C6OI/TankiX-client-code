using System;
using System.Collections;
using System.Text;
using System.Xml;
using log4net.Core;
using log4net.Util;

namespace log4net.Layout {
    public class XmlLayout : XmlLayoutBase {
        const string PREFIX = "log4net";

        const string ELM_EVENT = "event";

        const string ELM_MESSAGE = "message";

        const string ELM_PROPERTIES = "properties";

        const string ELM_GLOBAL_PROPERTIES = "global-properties";

        const string ELM_DATA = "data";

        const string ELM_EXCEPTION = "exception";

        const string ELM_LOCATION = "locationInfo";

        const string ATTR_LOGGER = "logger";

        const string ATTR_TIMESTAMP = "timestamp";

        const string ATTR_LEVEL = "level";

        const string ATTR_THREAD = "thread";

        const string ATTR_DOMAIN = "domain";

        const string ATTR_IDENTITY = "identity";

        const string ATTR_USERNAME = "username";

        const string ATTR_CLASS = "class";

        const string ATTR_METHOD = "method";

        const string ATTR_FILE = "file";

        const string ATTR_LINE = "line";

        const string ATTR_NAME = "name";

        const string ATTR_VALUE = "value";

        string m_elmData = "data";

        string m_elmEvent = "event";

        string m_elmException = "exception";

        string m_elmLocation = "locationInfo";

        string m_elmMessage = "message";

        string m_elmProperties = "properties";

        public XmlLayout() { }

        public XmlLayout(bool locationInfo)
            : base(locationInfo) { }

        public string Prefix { get; set; } = "log4net";

        public bool Base64EncodeMessage { get; set; }

        public bool Base64EncodeProperties { get; set; }

        public override void ActivateOptions() {
            base.ActivateOptions();

            if (Prefix != null && Prefix.Length > 0) {
                m_elmEvent = Prefix + ":event";
                m_elmMessage = Prefix + ":message";
                m_elmProperties = Prefix + ":properties";
                m_elmData = Prefix + ":data";
                m_elmException = Prefix + ":exception";
                m_elmLocation = Prefix + ":locationInfo";
            }
        }

        protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent) {
            writer.WriteStartElement(m_elmEvent);
            writer.WriteAttributeString("logger", loggingEvent.LoggerName);
            writer.WriteAttributeString("timestamp", XmlConvert.ToString(loggingEvent.TimeStamp));
            writer.WriteAttributeString("level", loggingEvent.Level.DisplayName);
            writer.WriteAttributeString("thread", loggingEvent.ThreadName);

            if (loggingEvent.Domain != null && loggingEvent.Domain.Length > 0) {
                writer.WriteAttributeString("domain", loggingEvent.Domain);
            }

            if (loggingEvent.Identity != null && loggingEvent.Identity.Length > 0) {
                writer.WriteAttributeString("identity", loggingEvent.Identity);
            }

            if (loggingEvent.UserName != null && loggingEvent.UserName.Length > 0) {
                writer.WriteAttributeString("username", loggingEvent.UserName);
            }

            writer.WriteStartElement(m_elmMessage);

            if (!Base64EncodeMessage) {
                Transform.WriteEscapedXmlString(writer, loggingEvent.RenderedMessage, InvalidCharReplacement);
            } else {
                byte[] bytes = Encoding.UTF8.GetBytes(loggingEvent.RenderedMessage);
                string textData = Convert.ToBase64String(bytes, 0, bytes.Length);
                Transform.WriteEscapedXmlString(writer, textData, InvalidCharReplacement);
            }

            writer.WriteEndElement();
            PropertiesDictionary properties = loggingEvent.GetProperties();

            if (properties.Count > 0) {
                writer.WriteStartElement(m_elmProperties);

                foreach (DictionaryEntry item in (IDictionary)properties) {
                    writer.WriteStartElement(m_elmData);

                    writer.WriteAttributeString("name",
                        Transform.MaskXmlInvalidCharacters((string)item.Key, InvalidCharReplacement));

                    string text = null;

                    if (!Base64EncodeProperties) {
                        text = Transform.MaskXmlInvalidCharacters(
                            loggingEvent.Repository.RendererMap.FindAndRender(item.Value),
                            InvalidCharReplacement);
                    } else {
                        byte[] bytes2 =
                            Encoding.UTF8.GetBytes(loggingEvent.Repository.RendererMap.FindAndRender(item.Value));

                        text = Convert.ToBase64String(bytes2, 0, bytes2.Length);
                    }

                    writer.WriteAttributeString("value", text);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            string exceptionString = loggingEvent.GetExceptionString();

            if (exceptionString != null && exceptionString.Length > 0) {
                writer.WriteStartElement(m_elmException);
                Transform.WriteEscapedXmlString(writer, exceptionString, InvalidCharReplacement);
                writer.WriteEndElement();
            }

            if (LocationInfo) {
                LocationInfo locationInformation = loggingEvent.LocationInformation;
                writer.WriteStartElement(m_elmLocation);
                writer.WriteAttributeString("class", locationInformation.ClassName);
                writer.WriteAttributeString("method", locationInformation.MethodName);
                writer.WriteAttributeString("file", locationInformation.FileName);
                writer.WriteAttributeString("line", locationInformation.LineNumber);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}