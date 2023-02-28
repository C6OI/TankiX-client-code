using System.IO;
using log4net.Util;
using SystemInfo = UnityEngine.SystemInfo;

namespace Platform.Tool.ClientUnityLogger.API {
    public class DeviceIdConverter : PatternConverter {
        public const string KEY = "deviceId";

        protected override void Convert(TextWriter writer, object state) {
            writer.Write(SystemInfo.deviceUniqueIdentifier);
        }
    }
}