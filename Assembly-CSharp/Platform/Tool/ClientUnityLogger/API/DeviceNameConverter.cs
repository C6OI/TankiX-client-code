using System.IO;
using log4net.Util;
using SystemInfo = UnityEngine.SystemInfo;

namespace Platform.Tool.ClientUnityLogger.API {
    public class DeviceNameConverter : PatternConverter {
        public const string KEY = "deviceName";

        protected override void Convert(TextWriter writer, object state) {
            writer.Write(SystemInfo.deviceName);
        }
    }
}