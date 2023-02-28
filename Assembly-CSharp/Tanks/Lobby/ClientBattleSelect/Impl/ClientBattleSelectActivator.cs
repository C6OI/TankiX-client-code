using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ClientBattleSelectActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.RegisterPart<BattleTemplatePart>();
            TemplateRegistry.Register<BattleSelectTemplate>();
            TemplateRegistry.Register<BattleSelectScreenTemplate>();
            TemplateRegistry.Register<BattleLobbyTemplate>();
            TemplateRegistry.Register<CustomBattleLobbyTemplate>();
            TemplateRegistry.Register<BattleLobbyChatTemplate>();
            TemplateRegistry.Register<SquadTemplate>();
            TemplateRegistry.Register<BattleResultRewardTemplate>();
            TemplateRegistry.Register<XCrystalBattleRewardTemplate>();
            TemplateRegistry.Register<ModuleContainerBattleRewardTemplate>();
            TemplateRegistry.Register<LeagueFirstEntranceRewardTemplate>();
            TemplateRegistry.Register<TutorialBattleRewardTemplate>();
            TemplateRegistry.Register<LevelUpUnlockBattleRewardTemplate>();
            TemplateRegistry.Register<PersonalBattleRewardTemplate>();
            TemplateRegistry.Register<XCrystalBonusPersonalBattleRewardTemplate>();
            EngineService.RegisterSystem(new BattleSelectSystem());
            EngineService.RegisterSystem(new UserReadyToBattleSystem());
            EngineService.RegisterSystem(new CreateCustomBattleSystem());
            EngineService.RegisterSystem(new BattleLobbyScreenSystem());
            EngineService.RegisterSystem(new BattleLobbyChatSystem());
            EngineService.RegisterSystem(new InviteToLobbySystem());
            EngineService.RegisterSystem(new ConnectToLobbyUiSystem());
            EngineService.RegisterSystem(new CommonScoreTableSystem());
            EngineService.RegisterSystem(new DMScoreTableSystem());
            EngineService.RegisterSystem(new TeamScoreTableSystem());
            EngineService.RegisterSystem(new ScoreTableEmptyRowsSystem());
            EngineService.RegisterSystem(new ScoreTableUserLabelIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableDeathsIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableKillsIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableScoreIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableEmptyRowIndicatorSystem());
            EngineService.RegisterSystem(new ShowIndicatorOnRoundRestartSystem());
            EngineService.RegisterSystem(new ScoreTableHullIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableTurretIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableFlagIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTablePingIndicatorSystem());
            EngineService.RegisterSystem(new RoundUserEquipmentSystem());
            EngineService.RegisterSystem(new JoinToSelectedBattleSystem());
            EngineService.RegisterSystem(new JoinToScreenBattleSystem());
            EngineService.RegisterSystem(new BattleTimeIndicatorSystem());
            EngineService.RegisterSystem(new BattleScoreLimitIndicatorSystem());
            EngineService.RegisterSystem(new TeamBattleScoreIndicatorSystem());
            EngineService.RegisterSystem(new BattleInfoSystem());
            EngineService.RegisterSystem(new BattleInfoEntranceSystem());
            EngineService.RegisterSystem(new EnterAsSpectatorSystem());
            EngineService.RegisterSystem(new BattleDetailsSystem());
            EngineService.RegisterSystem(new BattleInfoTeamViewSystem());
            EngineService.RegisterSystem(new BattleDetailsDMSystem());
            EngineService.RegisterSystem(new BattleDetailsTDMSystem());
            EngineService.RegisterSystem(new BattleSelectLoadSystem());
            EngineService.RegisterSystem(new BattleSelectInviteFriendsScreenSystem());
            EngineService.RegisterSystem(new BattleSelectInviteFriendsListSystem());
            EngineService.RegisterSystem(new InviteFriendToBattleNotificationSystem());
            TemplateRegistry.Register<InviteFriendToBattleNotificationTemplate>();
            EngineService.RegisterSystem(new ReturnToBattleSystem());
            EngineService.RegisterSystem(new MatchLobbyGUISystem());
            EngineService.RegisterSystem(new MatchLobbySoundSystem());
            EngineService.RegisterSystem(new ECSDumperSystem());
            EngineService.RegisterSystem(new UserInteractionsSystem());
            EngineService.RegisterSystem(new SquadInfoSystem());
            EngineService.RegisterSystem(new SquadSystem());
            EngineService.RegisterSystem(new InviteToSquadSystem());
            EngineService.RegisterSystem(new RequestToSquadSystem());
            EngineService.RegisterSystem(new SquadInteractionSystem());
            EngineService.RegisterSystem(new LeaveSquadSystem());
            EngineService.RegisterSystem(new SquadColorSystem());
            EngineService.RegisterSystem(new BattleResultCommonScreenSystem());
            EngineService.RegisterSystem(new BattleResultAwardsScreenSystem());
            EngineService.RegisterSystem(new BattleResultStatScreenSystem());
            EngineService.RegisterSystem(new BattleSeriesUiSystem());
            EngineService.RegisterSystem(new XCrystalBonusRewardsSystem());
            EngineService.RegisterSystem(new ModuleContainerRewardSystem());
            EngineService.RegisterSystem(new LevelUpUnlockRewardSystem());
            EngineService.RegisterSystem(new BattleCoverSystem());
            EngineService.RegisterSystem(new BattleResultsScreenSystem());
            EngineService.RegisterSystem(new SpecialOfferUiSystem());
            MapRegistrySystem mapRegistrySystem = new();
            EngineService.RegisterSystem(mapRegistrySystem);
            ServiceRegistry.Current.RegisterService((MapRegistry)mapRegistrySystem);
        }
    }
}