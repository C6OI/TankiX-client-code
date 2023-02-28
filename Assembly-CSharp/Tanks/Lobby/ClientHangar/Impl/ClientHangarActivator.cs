using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientHangar.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ClientHangarActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static InputManager InputManager { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new AssetsFirstLoadingSystem());
            EngineService.RegisterSystem(new HangarBuilderSystem());
            EngineService.RegisterSystem(new HangarCameraSwitchSystem());
            EngineService.RegisterSystem(new HangarTankBuilderSystem());
            EngineService.RegisterSystem(new HangarTankLoadSystem());
            EngineService.RegisterSystem(new ItemPreviewBaseSystem());
            EngineService.RegisterSystem(new ItemPreviewSystem());
            EngineService.RegisterSystem(new ContainerItemPreviewSystem());
            EngineService.RegisterSystem(new HangarGraffitiBuilderSystem());
            EngineService.RegisterSystem(new HangarContainerBuilderSystem());
            EngineService.RegisterSystem(new HangarCameraControlSystem());
            EngineService.RegisterSystem(new HangarCameraRotateSystem());
            EngineService.RegisterSystem(new HangarCameraAutoRotateSystem());
            EngineService.RegisterSystem(new UserReadyForLobbySystem());
            EngineService.RegisterSystem(new HangarAmbientSoundSystem());
            EngineService.RegisterSystem(new CardsContainerSoundSystem());
            EngineService.RegisterSystem(new HangarModuleSoundsSystem());
            EngineService.RegisterSystem(new HangarCameraFlightToLocationSystem());
            EngineService.RegisterSystem(new HangarCameraFlightToTankSystem());
            EngineService.RegisterSystem(new HangarCameraFlightSystem());
            TemplateRegistry.Register(typeof(HangarTemplate));
        }

        protected override void Activate() {
            CreateEntity<HangarTemplate>("/hangar");
        }
    }
}