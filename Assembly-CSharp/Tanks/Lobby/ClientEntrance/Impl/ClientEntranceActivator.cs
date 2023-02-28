using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class ClientEntranceActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterTemplates();
            RegisterSystems();
        }

        void RegisterTemplates() {
            TemplateRegistry.Register<LobbyTemplate>();
            TemplateRegistry.Register<AuthentificationTemplate>();
            TemplateRegistry.Register<RanksExperiencesConfigTemplate>();
            TemplateRegistry.Register<RanksNamesTemplate>();
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new SelfUserSystem());
            EngineService.RegisterSystem(new UserMoneyIndicatorSystem());
            EngineService.RegisterSystem(new DependentInteractivitySystem());
            EngineService.RegisterSystem(new EntryPointSystem());
            EngineService.RegisterSystem(new EntranceSystem());
            EngineService.RegisterSystem(new EntranceScreenSystem());
            EngineService.RegisterSystem(new EntranceInputValidationSystem());
            EngineService.RegisterSystem(new RegistrationScreenSystem());
            EngineService.RegisterSystem(new RegistrationInputValidationSystem());
            EngineService.RegisterSystem(new InviteScreenSystem());
            EngineService.RegisterSystem(new InviteInputValidationSystem());
            EngineService.RegisterSystem(new SaveLoginSystem());
            EngineService.RegisterSystem(new EntranceStatisticsSystem());
            EngineService.RegisterSystem(new UidInputValidationSystem());
            EngineService.RegisterSystem(new CodeInputValidationSystem());
            EngineService.RegisterSystem(new SelfUserToLoggerSystem());
            EngineService.RegisterSystem(new SubscribeCheckboxSystem());
            EngineService.RegisterSystem(new SteamAuthenticationSystem());
        }

        protected override void Activate() {
            Engine engine = EngineService.Engine;
            engine.CreateEntity(typeof(AuthentificationTemplate), "/lobby/entrance/authentication");
            engine.CreateEntity(typeof(RanksExperiencesConfigTemplate), "/ranksconfig");
            engine.CreateEntity(typeof(RanksNamesTemplate), "/ui/element/ranksnames");
        }
    }
}