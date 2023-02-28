using System;
using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Platform.Tool.ClientUnityLogger.API {
    public class UNIXepochTimeStampConverter : PatternLayoutConverter {
        public const string KEY = "UNIXepochTimeStamp";

        static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
            writer.Write(loggingEvent.TimeStamp.ToUniversalTime().Subtract(UnixEpoch).TotalSeconds);
        }
    }
}