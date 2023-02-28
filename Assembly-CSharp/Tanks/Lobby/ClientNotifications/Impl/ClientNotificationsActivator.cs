using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class ClientNotificationsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<IgnoreBattleScreenNotificationTemplate>();
            TemplateRegistry.Register<LockScreenNotificationTemplate>();
            TemplateRegistry.Register<UIDChangedNotificationTemplate>();
            TemplateRegistry.Register<IgnoreBattleResultScreenNotificationTemplate>();
            TemplateRegistry.Register<UserRankRewardNotificationTemplate>();
            TemplateRegistry.Register<NewItemNotificationTemplate>();
            TemplateRegistry.Register<NewItemClientNotificationTemplate>();
            TemplateRegistry.Register<LeagueFirstEntranceRewardPersistentNotificationTemplate>();
            TemplateRegistry.Register<LeagueSeasonEndRewardPersistentNotificationTemplate>();
            TemplateRegistry.Register<EnergyCompensationPersistentNotificationTemplate>();
            TemplateRegistry.Register<EventContainerPersistentNotificationTemplate>();
            TemplateRegistry.Register<ReleaseGiftsPersistentNotificationTemplate>();
            TemplateRegistry.Register<EulaPersistentNotificationTemplate>();
            TemplateRegistry.Register<LoginRewardNotificationTemplate>();
            TemplateRegistry.Register<PrivacyPolicyPersistentNotificationTemplate>();
            TemplateRegistry.Register<FractionsCompetitionStartNotificationTemplate>();
            TemplateRegistry.Register<FractionsCompetitionRewardNotificationTemplate>();
            EngineService.RegisterSystem(new UIDChangedNotificationSystem());
            EngineService.RegisterSystem(new UserRankRewardNotificationSystem());
            EngineService.RegisterSystem(new IgnoreBattleScreenNotificationSystem());
            EngineService.RegisterSystem(new LockScreenNotificationSystem());
            EngineService.RegisterSystem(new CancelNotificationSystem());
            EngineService.RegisterSystem(new IgnoreBattleResultsScreenNotificationSystem());
            EngineService.RegisterSystem(new NewItemNotificationSystem());
            EngineService.RegisterSystem(new NotificationsOnBattleResultsScreenSystem());
            EngineService.RegisterSystem(new LeagueFirstEntranceRewardNotificationSystem());
            EngineService.RegisterSystem(new LeagueSeasonEndRewardNotificationSystem());
            EngineService.RegisterSystem(new EnergyCompensationNotificationSystem());
            EngineService.RegisterSystem(new EulaNotificationSystem());
            EngineService.RegisterSystem(new EventContainerScreenSystem());
            EngineService.RegisterSystem(new PrivacyPolicyNotificationSystem());
        }
    }
}