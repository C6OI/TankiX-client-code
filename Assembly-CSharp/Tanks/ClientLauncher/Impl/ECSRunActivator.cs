using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Tanks.ClientLauncher.Impl {
    public class ECSRunActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RunECSKernel();
        }
    }
}