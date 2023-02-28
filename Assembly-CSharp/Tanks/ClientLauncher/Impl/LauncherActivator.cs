using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<LauncherDownloadScreenTemplate>();
            EngineService.RegisterSystem(new LauncherScreensSystem());
        }
    }
}