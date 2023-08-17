using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigurationServiceActivator : DefaultActivator<AutoCompleting> {
        protected override void Activate() =>
            ServiceRegistry.Current.RegisterService((ConfigurationService)new ConfigurationServiceImpl());
    }
}