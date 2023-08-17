using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientHangar.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ClientHangarActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static InputManager InputManager { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new HangarBuilderSystem());
            EngineService.RegisterSystem(new HangarCameraSwitchSystem());
            EngineService.RegisterSystem(new HangarTankBuilderSystem());
            EngineService.RegisterSystem(new HangarTankLoadSystem());
            EngineService.RegisterSystem(new HangarTankSupplySystem());
            EngineService.RegisterSystem(new ItemPreviewSystem());
            EngineService.RegisterSystem(new HangarGraffitiBuilderSystem());
            EngineService.RegisterSystem(new HangarCameraControlSystem());
            EngineService.RegisterSystem(new HangarCameraRotateSystem());
            EngineService.RegisterSystem(new HangarCameraAutoRotateSystem());
            EngineService.RegisterSystem(new UserReadyForLobbySystem());
            EngineService.RegisterSystem(new HangarAmbientSoundSystem());
            TemplateRegistry.Register(typeof(HangarTemplate));
        }

        protected override void Activate() => EngineService.ExecuteInFlow(delegate(Engine e) {
            e.CreateEntity<HangarTemplate>("/hangar");
        });
    }
}