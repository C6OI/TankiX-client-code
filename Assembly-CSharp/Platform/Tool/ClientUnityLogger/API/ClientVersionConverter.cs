using System.IO;
using log4net.Util;
using Platform.Library.ClientUnityIntegration.API;

namespace Platform.Tool.ClientUnityLogger.API {
    public class ClientVersionConverter : PatternConverter {
        public const string KEY = "ClientVersion";

        protected override void Convert(TextWriter writer, object state) {
            if (StartupConfiguration.Config != null) {
                writer.Write(StartupConfiguration.Config.CurrentClientVersion);
            }
        }
    }
}