using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.System.Data.Statics.ClientYaml.Impl {
    public class YamlActivator : DefaultActivator<AutoCompleting> {
        protected override void Activate() {
            ServiceRegistry.Current.RegisterService((YamlService)new YamlServiceImpl());
        }
    }
}