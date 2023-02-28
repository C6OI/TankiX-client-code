using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserProfileActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<DailyBonusConfigTemplate>();
            TemplateRegistry.RegisterPart<ClientSessionTemplatePart>();
            TemplateRegistry.Register<ProfileScreenTemplate>();
            TemplateRegistry.Register<UserTemplate>();
            TemplateRegistry.RegisterPart<UserNotificationsTemplatePart>();
            TemplateRegistry.Register<NewsItemTemplate>();
            TemplateRegistry.Register<ConfiguredNewsItemTemplate>();
            TemplateRegistry.Register<EmailConfirmationNotificationTemplate>();
            TemplateRegistry.Register<SimpleTextNotificationTemplate>();
            TemplateRegistry.Register<EmailConfigTemplate>();
            TemplateRegistry.Register<LeagueTemplate>();
            TemplateRegistry.Register<LeaguesConfigTemplate>();
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
            EngineService.RegisterSystem(new UserExperienceIndicatorSystem());
            EngineService.RegisterSystem(new ProfileScreenNavigationSystem());
            EngineService.RegisterSystem(new ProfileScreenLoadSystem());
            EngineService.RegisterSystem(new ProfileScreenSystem());
            EngineService.RegisterSystem(new ChangeUserEmailScreenSystem());
            EngineService.RegisterSystem(new ConfirmUserEmailScreenSystem());
            EngineService.RegisterSystem(new EmailConfirmationNotificationSystem());
            EngineService.RegisterSystem(new ViewUserEmailScreenSystem());
            EngineService.RegisterSystem(new EnterUserEmailScreenSystem());
            EngineService.RegisterSystem(new EnterConfirmationCodeScreenSystem());
            EngineService.RegisterSystem(new EnterNewPasswordScreenSystem());
            EngineService.RegisterSystem(new HelpButtonSystem());
            EngineService.RegisterSystem(new NewsSystem());
            EngineService.RegisterSystem(new LeaguesSystem());
            EngineService.RegisterSystem(new TopLeagueInfoSystem());
            TemplateRegistry.RegisterPart<UserLeagueTemplatePart>();
            TemplateRegistry.RegisterPart<GameplayChestUserTemplatePart>();
            TemplateRegistry.Register<FractionTemplate>();
            TemplateRegistry.Register<FractionsCompetitionTemplate>();
            EngineService.RegisterSystem(new FractionsCompetitionSystem());
            EngineService.RegisterSystem(new FractionsCompetitionUiSystem());
        }

        protected override void Activate() {
            Engine engine = EngineService.Engine;
            engine.CreateEntity(typeof(EmailConfigTemplate), "lobby/userprofile/email/configuration");
            engine.CreateEntity(typeof(DailyBonusConfigTemplate), "dailybonus");
        }
    }
}