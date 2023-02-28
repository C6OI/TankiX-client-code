using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ClientProtocolActivator : DefaultActivator<AutoCompleting> {
        protected override void Activate() {
            ServiceRegistry.Current.RegisterService((Protocol)new ProtocolImpl());
        }
    }
}