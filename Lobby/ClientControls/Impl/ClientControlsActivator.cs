using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Lobby.ClientControls.Impl {
    public class ClientControlsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new CommonControlsSystem());
            EngineService.RegisterSystem(new InputFieldSystem());
            EngineService.RegisterSystem(new CaptchaSystem());
            EngineService.RegisterSystem(new LoadGearSystem());
            EngineService.RegisterSystem(new ScreenForegroundSystem());
            EngineService.RegisterSystem(new CarouselSystem());
            TemplateRegistry.Register(typeof(LocalizedTextTemplate));
            TemplateRegistry.Register(typeof(CarouselItemTemplate));
        }
    }
}