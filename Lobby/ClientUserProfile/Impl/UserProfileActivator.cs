using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Lobby.ClientUserProfile.Impl {
    public class UserProfileActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.RegisterPart<ClientSessionTemplatePart>();
            TemplateRegistry.Register<ProfileScreenTemplate>();
            TemplateRegistry.RegisterPart<UserNotificationsTemplatePart>();
            EngineService.RegisterSystem(new LoadUsersSystem());
            EngineService.RegisterSystem(new UserLabelSystem());
            EngineService.RegisterSystem(new UserLabelLoadSystem());
            EngineService.RegisterSystem(new RankIconSystem());
            EngineService.RegisterSystem(new UserLabelUidSystem());
            EngineService.RegisterSystem(new ClientLocaleSystem());
            EngineService.RegisterSystem(new NotificationSystem());
            EngineService.RegisterSystem(new EmailInputValidationSystem());
            EngineService.RegisterSystem(new JoinScreenElementToUserSystem());
            EngineService.RegisterSystem(new RankNameSystem());
            EngineService.RegisterSystem(new LogoutSystem());
            EngineService.RegisterSystem(new UserExperienceIndicatorSystem());
            EngineService.RegisterSystem(new ProfileScreenNavigationSystem());
            EngineService.RegisterSystem(new ProfileScreenLoadSystem());
            EngineService.RegisterSystem(new ProfileScreenSystem());
            EngineService.RegisterSystem(new ChangeUserEmailScreenSystem());
            EngineService.RegisterSystem(new ConfirmUserEmailScreenSystem());
            EngineService.RegisterSystem(new ViewUserEmailScreenSystem());
            EngineService.RegisterSystem(new EnterUserEmailScreenSystem());
            EngineService.RegisterSystem(new EnterConfirmationCodeScreenSystem());
            EngineService.RegisterSystem(new EnterNewPasswordScreenSystem());
            EngineService.RegisterSystem(new HelpButtonSystem());
        }
    }
}