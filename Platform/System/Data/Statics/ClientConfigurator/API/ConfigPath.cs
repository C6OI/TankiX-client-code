using System;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public class ConfigPath {
        public static readonly string CLIENT_LOGGER_CONFIG = "log4net.xml";

        public static readonly string CONFIG_FILE_NAME = "config.tar";

        public static readonly string CONFIG = "config";

        public static readonly string CLIENT_LOCAL = "clientlocal";

        public static readonly string STARTUP = CLIENT_LOCAL + "/startup";

        public static readonly string DEFAULT_LOCALE = CLIENT_LOCAL + "/locale";

        public static readonly string LOADING_STOP_TIMEOUT = CLIENT_LOCAL + "/loadingstoptimeout";

        public static readonly string CLIENT_RESOURCES = CLIENT_LOCAL + "/clientresources";

        public static string ConvertToUrl(string path) {
            Uri uri = new(path);

            if (uri.Scheme.Contains("http")) {
                return path;
            }

            return "file:///" + path;
        }
    }
}