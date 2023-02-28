using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientHUD.API;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientMatchMaking.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleScreenSystem : ECSSystem {
        [OnEventFire]
        public void HideCursor(NodeAddedEvent e, SingleNode<BattleScreenComponent> battleLoadScreen, [JoinAll] UserAsTank selfBattleUserAsTank) {
            ScheduleEvent<BattleFullyLoadedEvent>(selfBattleUserAsTank);
        }

        [OnEventFire]
        public void GroupScoreTable(NodeAddedEvent e, ReadyBattleUserCommon battleUser, ScreenInitNode screen) {
            screen.Entity.AddComponent(new BattleGroupComponent(battleUser.battleGroup.Key));
        }

        [OnEventFire]
        public void ShowBattleScreen(NodeAddedEvent e, ReadyBattleUser battleUser, [Context] [JoinByBattle] DMBattleNode dmBattle) {
            ScheduleEvent<ShowScreenNoAnimationEvent<DMBattleScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ShowBattleScreen(NodeAddedEvent e, ReadyBattleUser battleUser, [Context] [JoinByBattle] TeamBattleNode teamBattle) {
            ScheduleEvent<ShowScreenNoAnimationEvent<TeamBattleScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ShowDMBattleSpectatorScreen(NodeAddedEvent e, ReadySpectator battleUser, [JoinByBattle] SingleNode<DMComponent> dmBattle) {
            ScheduleEvent<ShowScreenNoAnimationEvent<DMBattleScpectatorScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ShowTeamBattleSpectatorScreen(NodeAddedEvent e, ReadySpectator battleUser, [JoinByBattle] SingleNode<TeamBattleComponent> teamBattle) {
            ScheduleEvent<ShowScreenNoAnimationEvent<TeamBattleSpectatorScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ExitBattle(NodeRemoveEvent e, SingleNode<BattleScreenComponent> battleScreen, [JoinAll] SingleNode<SelfBattleUserComponent> selfBattleUser) {
            ScheduleEvent<ExitBattleEvent>(selfBattleUser);
        }

        [OnEventFire]
        public void ExitBattleOnLeaveLoad(NodeRemoveEvent e, SingleNode<BattleLoadScreenComponent> battleLoadScreen, [JoinAll] NotReadySelfBattleUserNode selfBattleUser) {
            ScheduleEvent<ExitBattleEvent>(selfBattleUser);
        }

        [OnEventFire]
        public void ExitBattleOnLoading(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleLoadScreenComponent> battleLoadScreen) {
            ScheduleEvent<GoBackFromBattleEvent>(selfBattleUser);
        }

        [OnEventFire]
        public void OnRequestGoBack(RequestGoBackFromBattleEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ActiveBattleScreenNode screen,
            [JoinAll] SelfBattleNonSpectatorNode battleUser, [JoinByBattle] SingleNode<RoundActiveStateComponent> activeRound, [JoinByBattle] BattleNode battle,
            [JoinByBattle] ICollection<BattleUserAsTankNode> battleUsers, [JoinAll] Optional<SingleNode<CustomBattleLobbyComponent>> customLobby,
            [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode) {
            bool isDeserter = IsDeserter(battle, battleUser, battleUsers);
            bool isNewbieBattle = chosenMode.IsPresent() && chosenMode.Get().component.ModeEntity.HasComponent<MatchMakingDefaultModeComponent>();
            dialogs.component.Get<ExitBattleWindow>().Show(screen.Entity, battleUser.Entity, customLobby.IsPresent(), isDeserter, isNewbieBattle);
        }

        bool IsDeserter(BattleNode battle, SelfBattleUserNode battleUser, ICollection<BattleUserAsTankNode> battleUsers) {
            Entity entity = battle.Entity;
            Entity entity2 = battleUser.Entity;

            if (entity.HasComponent<DMComponent>()) {
                return battleUsers.Count > 1;
            }

            if (!entity.HasComponent<TeamBattleComponent>()) {
                return false;
            }

            int num = 0;

            foreach (BattleUserAsTankNode battleUser2 in battleUsers) {
                Entity entity3 = battleUser2.Entity;

                if (!entity2.IsSameGroup<TeamGroupComponent>(entity3) && !entity3.HasComponent<TankAutopilotComponent>()) {
                    num++;
                }
            }

            return num > 0;
        }

        [OnEventFire]
        public void GoBackOnKick(KickFromBattleEvent e, SelfBattleUserNode battleUser, [JoinByBattle] SingleNode<RoundComponent> round,
            [JoinAll] Optional<SingleNode<BattleResultsComponent>> battleResults) {
            if (!battleResults.IsPresent()) {
                ScheduleEvent<GoBackFromBattleEvent>(battleUser);
            }
        }

        [OnEventFire]
        public void GoBack(GoBackFromBattleEvent e, Node any) {
            ScheduleEvent<ShowScreenNoAnimationEvent<ExitBattleToLobbyLoadScreenComponent>>(any);
        }

        [OnEventFire]
        public void ShowGarage(NodeAddedEvent e, SingleNode<ExitBattleToLobbyLoadScreenComponent> screen, UserReadyForLobbyNode user) {
            ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(user);
            ScheduleEvent<GoBackFromBattleScreensEvent>(screen);
        }

        [OnEventFire]
        public void ShowBackgroundAndLoadHangar(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round) {
            if ((bool)Camera.main) {
                BlurOptimized component = Camera.main.gameObject.GetComponent<BlurOptimized>();

                if (component != null) {
                    component.enabled = true;
                    component.blurSize = 3f;
                }
            }
        }

        [OnEventFire]
        public void ShowGoBackRequest(SpectatorGoBackRequestEvent e, Node anyNode, [JoinAll] FreeCameraNode camera, [JoinAll] SelfBattleUserNode battleUser,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ActiveBattleScreenNode screen, [JoinAll] Optional<SingleNode<CustomBattleLobbyComponent>> customLobby) {
            dialogs.component.Get<ExitBattleWindow>().Show(screen.Entity, battleUser.Entity, customLobby.IsPresent(), false, false);
        }

        [OnEventFire]
        public void ShowResultTitle(NodeAddedEvent e, SingleNode<BattleResultsComponent> results, [JoinAll] SingleNode<MainHUDComponent> hud,
            [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            hud.component.Hide();
            BattleResultForClient resultForClient = results.component.ResultForClient;

            if (resultForClient.PersonalResult == null) {
                return;
            }

            if (resultForClient.BattleMode == BattleMode.DM) {
                dialogs.component.Get<DMFinishWindow>().Show();
                return;
            }

            TeamFinishWindow teamFinishWindow = dialogs.component.Get<TeamFinishWindow>();
            teamFinishWindow.CustomBattle = resultForClient.Custom;

            if (resultForClient.PersonalResult.TeamBattleResult == TeamBattleResult.DRAW) {
                teamFinishWindow.ShowTie();
            } else if (resultForClient.PersonalResult.TeamBattleResult == TeamBattleResult.WIN) {
                teamFinishWindow.ShowWin();
            } else {
                teamFinishWindow.ShowLose();
            }
        }

        public class BattleUserAsTankNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleUserComponent battleUser;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class SelfBattleNonSpectatorNode : SelfBattleUserNode { }

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

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
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

        public class FreeCameraNode : Node {
            public FreeCameraComponent freeCamera;
            public SpectatorCameraComponent spectatorCamera;
        }

        public class UserReadyForLobbyNode : Node {
            public SelfUserComponent selfUser;

            public UserReadyForLobbyComponent userReadyForLobby;
        }

        [Not(typeof(UserReadyToBattleComponent))]
        public class NotReadySelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;
        }

        [Not(typeof(BattleLoadScreenComponent))]
        public class ActiveBattleScreenNode : Node {
            public ActiveScreenComponent activeScreen;
        }
    }
}