using log4net.Util;

namespace log4net {
    public sealed class GlobalContext {
        static GlobalContext() {
            Properties = new GlobalContextProperties();
            Properties["log4net:HostName"] = SystemInfo.HostName;
        }

        GlobalContext() { }

        public static GlobalContextProperties Properties { get; }
    }
}