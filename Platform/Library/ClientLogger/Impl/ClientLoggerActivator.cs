using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;

namespace Platform.Library.ClientLogger.Impl {
    public class ClientLoggerActivator : DefaultActivator<AutoCompleting> {
        protected override void Activate() => LoggerProvider.Init();
    }
}