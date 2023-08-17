using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ClientBattleSelectActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.RegisterPart<BattleTemplatePart>();
            TemplateRegistry.Register<BattleSelectTemplate>();
            TemplateRegistry.Register<BattleSelectScreenTemplate>();
            EngineService.RegisterSystem(new BattleSelectSystem());
            EngineService.RegisterSystem(new UserReadyToBattleSystem());
            EngineService.RegisterSystem(new CommonScoreTableSystem());
            EngineService.RegisterSystem(new DMScoreTableSystem());
            EngineService.RegisterSystem(new TeamScoreTableSystem());
            EngineService.RegisterSystem(new ScoreTableEmptyRowsSystem());
            EngineService.RegisterSystem(new ScoreTableUserLabelIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableDeathsIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableKillsIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableScoreIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTablePrizeIndicatorSystem());
            EngineService.RegisterSystem(new ScoreTableEmptyRowIndicatorSystem());
            EngineService.RegisterSystem(new ShowIndicatorOnRoundRestartSystem());
            EngineService.RegisterSystem(new JoinToSelectedBattleSystem());
            EngineService.RegisterSystem(new JoinToScreenBattleSystem());
            EngineService.RegisterSystem(new BattleTimeIndicatorSystem());
            EngineService.RegisterSystem(new BattleScoreLimitIndicatorSystem());
            EngineService.RegisterSystem(new TeamBattleScoreIndicatorSystem());
            EngineService.RegisterSystem(new BattleInfoSystem());
            EngineService.RegisterSystem(new BattleInfoEntranceSystem());
            EngineService.RegisterSystem(new BattleDetailsSystem());
            EngineService.RegisterSystem(new BattleInfoTeamViewSystem());
            EngineService.RegisterSystem(new BattleDetailsDMSystem());
            EngineService.RegisterSystem(new BattleDetailsTDMSystem());
            EngineService.RegisterSystem(new BattleSelectLoadSystem());
            EngineService.RegisterSystem(new BattleSelectInviteFriendsScreenSystem());
            EngineService.RegisterSystem(new BattleSelectInviteFriendsListSystem());
            EngineService.RegisterSystem(new InviteFriendToBattleNotificationSystem());
            TemplateRegistry.Register<InviteFriendToBattleNotificationTemplate>();
        }
    }
}