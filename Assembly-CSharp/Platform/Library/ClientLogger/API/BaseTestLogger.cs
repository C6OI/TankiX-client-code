using log4net;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Library.ClientLogger.API {
    public class BaseTestLogger {
        public BaseTestLogger() {
            LoggerProvider.Init();
        }

        [Inject] public static ILog Log { get; set; }
    }
}