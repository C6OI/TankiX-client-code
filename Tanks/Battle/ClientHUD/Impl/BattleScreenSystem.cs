using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleScreenSystem : ECSSystem {
        [OnEventFire]
        public void HideCursor(NodeAddedEvent e, SingleNode<BattleScreenComponent> battleLoadScreen,
            [JoinAll] UserAsTank selfBattleUserAsTank) => ScheduleEvent<BattleFullyLoadedEvent>(selfBattleUserAsTank);

        [OnEventFire]
        public void GroupScoreTable(NodeAddedEvent e, ReadyBattleUserCommon battleUser, ScreenInitNode screen) =>
            screen.Entity.AddComponent(new BattleGroupComponent(battleUser.battleGroup.Key));

        [OnEventFire]
        public void ShowBattleScreen(NodeAddedEvent e, ReadyBattleUser battleUser,
            [JoinByBattle] [Context] DMBattleNode dmBattle) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<DMBattleScreenComponent>>(battleUser);

        [OnEventFire]
        public void ShowBattleScreen(NodeAddedEvent e, ReadyBattleUser battleUser,
            [JoinByBattle] [Context] TeamBattleNode teamBattle) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<TeamBattleScreenComponent>>(battleUser);

        [OnEventFire]
        public void ShowDMBattleSpectatorScreen(NodeAddedEvent e, ReadySpectator battleUser,
            [JoinByBattle] SingleNode<DMComponent> dmBattle) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<DMBattleScpectatorScreenComponent>>(battleUser);

        [OnEventFire]
        public void ShowTeamBattleSpectatorScreen(NodeAddedEvent e, ReadySpectator battleUser,
            [JoinByBattle] SingleNode<TeamBattleComponent> teamBattle) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<TeamBattleSpectatorScreenComponent>>(battleUser);

        [OnEventFire]
        public void ExitBattle(NodeRemoveEvent e, SingleNode<BattleScreenComponent> battleScreen,
            [JoinAll] SingleNode<SelfBattleUserComponent> selfBattleUser) => ScheduleEvent<ExitBattleEvent>(selfBattleUser);

        [OnEventFire]
        public void OnRequestGoBack(RequestGoBackFromBattleEvent e, Node any, [JoinAll] SingleNode<DialogsComponent> dialogs,
            [JoinAll] SingleNode<ActiveScreenComponent> screen, [JoinAll] SingleNode<SelfBattleUserComponent> user) =>
            dialogs.component.Get<ExitBattleWindow>().Show(screen.Entity, user.Entity);

        [OnEventFire]
        public void GoBackOnKick(KickFromBattleEvent e, SingleNode<SelfBattleUserComponent> battleUser,
            [JoinByUser] [Mandatory] SingleNode<ClientSessionComponent> session) =>
            NewEvent<GoBackFromBattleEvent>().Attach(battleUser).Schedule();

        public class ReadyBattleUserCommon : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ReadyBattleUser : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsTankComponent userInBattleAsTank;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class DMBattleNode : Node {
            public BattleGroupComponent battleGroup;
            public DMComponent dm;
        }

        public class TeamBattleNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamBattleComponent teamBattle;
        }

        public class ReadySpectator : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ScreenInitNode : Node {
            public BattleScreenComponent battleScreen;
            public ScreenComponent screen;
        }

        public class UserAsTank : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }
    }
}