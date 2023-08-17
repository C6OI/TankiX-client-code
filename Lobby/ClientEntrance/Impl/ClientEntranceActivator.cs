using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Lobby.ClientEntrance.Impl {
    public class ClientEntranceActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterTemplates();
            RegisterSystems();
        }

        void RegisterTemplates() {
            TemplateRegistry.Register<LobbyTemplate>();
            TemplateRegistry.Register<UserTemplate>();
            TemplateRegistry.Register<AuthentificationTemplate>();
            TemplateRegistry.Register<RanksExperiencesConfigTemplate>();
            TemplateRegistry.Register<RanksNamesTemplate>();
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new SelfUserSystem());
            EngineService.RegisterSystem(new UserMoneyIndicatorSystem());
            EngineService.RegisterSystem(new DependentInteractivitySystem());
            EngineService.RegisterSystem(new EntryPointSystem());
            EngineService.RegisterSystem(new EntranceScreenSystem());
            EngineService.RegisterSystem(new EntranceInputValidationSystem());
            EngineService.RegisterSystem(new RegistrationScreenSystem());
            EngineService.RegisterSystem(new RegistrationInputValidationSystem());
            EngineService.RegisterSystem(new InviteScreenSystem());
            EngineService.RegisterSystem(new InviteInputValidationSystem());
            EngineService.RegisterSystem(new SaveLoginSystem());
            EngineService.RegisterSystem(new EntranceStatisticsSystem());
            EngineService.RegisterSystem(new EnterRegistrationCodeScreenSystem());
            EngineService.RegisterSystem(new ChangeUserNameScreenSystem());
            EngineService.RegisterSystem(new UidInputValidationSystem());
            EngineService.RegisterSystem(new CodeInputValidationSystem());
        }

        protected override void Activate() => EngineService.ExecuteInFlow(delegate(Engine engine) {
            engine.CreateEntity(typeof(AuthentificationTemplate), "/lobby/entrance/authentication");
            engine.CreateEntity(typeof(RanksExperiencesConfigTemplate), "/ranksexperiences");
            engine.CreateEntity(typeof(RanksNamesTemplate), "/ui/element/ranksnames");
        });
    }
}