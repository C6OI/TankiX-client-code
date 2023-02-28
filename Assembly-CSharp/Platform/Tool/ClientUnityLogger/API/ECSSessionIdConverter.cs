using System.IO;
using log4net.Util;
using Platform.Library.ClientUnityIntegration.API;

namespace Platform.Tool.ClientUnityLogger.API {
    public class ECSSessionIdConverter : PatternConverter {
        public const string KEY = "ECSSessionId";

        protected override void Convert(TextWriter writer, object state) {
            writer.Write(ECStoLoggerGateway.ClientSessionId);
        }
    }
}