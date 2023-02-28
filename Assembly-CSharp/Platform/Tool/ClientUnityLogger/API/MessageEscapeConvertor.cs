using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;
using Platform.Tool.ClientUnityLogger.Impl;

namespace Platform.Tool.ClientUnityLogger.API {
    public class MessageEscapeConvertor : PatternLayoutConverter {
        public const string KEY = "escapedMessage";

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            writer.Write(JsonUtil.ToJSONString(loggingEvent.MessageObject.ToString()));
        }
    }
}