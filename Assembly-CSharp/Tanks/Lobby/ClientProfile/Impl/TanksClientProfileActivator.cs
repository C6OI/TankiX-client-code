using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class TanksClientProfileActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new GameSettingsScreenSystem());
            EngineService.RegisterSystem(new SoundSettingsScreenSystem());
            EngineService.RegisterSystem(new SoundSettingsSystem());
            EngineService.RegisterSystem(new SfxVolumeSliderBarSystem());
            EngineService.RegisterSystem(new TanksSettingsSystem());
            EngineService.RegisterSystem(new CameraShakerSettingsSystem());
            EngineService.RegisterSystem(new TargetFocusSettingsSystem());
            EngineService.RegisterSystem(new LaserSightSettingsSystem());
            EngineService.RegisterSystem(new UserXCrystalsIndicatorSystem());
            EngineService.RegisterSystem(new MouseSettingsSystem());
            EngineService.RegisterSystem(new CBQAchievementSystem());
            TemplateRegistry.RegisterPart<TanksSettingsTemplatePart>();
            TemplateRegistry.RegisterPart<MouseSettingsTemplatePart>();
            TemplateRegistry.RegisterPart<GameCameraShakerSettingsTemplatePart>();
            TemplateRegistry.RegisterPart<TargetFocusSettingsTemplatePart>();
            EngineService.RegisterSystem(new CrystalsBufferSystem());
        }

        protected override void Activate() { }
    }
}