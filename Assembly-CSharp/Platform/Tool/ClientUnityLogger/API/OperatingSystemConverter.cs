using System.IO;
using log4net.Util;
using SystemInfo = UnityEngine.SystemInfo;

namespace Platform.Tool.ClientUnityLogger.API {
    public class OperatingSystemConverter : PatternConverter {
        public const string KEY = "operatingSystem";

        protected override void Convert(TextWriter writer, object state) {
            writer.Write(SystemInfo.operatingSystem);
        }
    }
}