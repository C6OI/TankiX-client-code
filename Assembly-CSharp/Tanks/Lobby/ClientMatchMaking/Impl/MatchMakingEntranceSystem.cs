using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientMatchMaking.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class MatchMakingEntranceSystem : ECSSystem {
        [OnEventFire]
        public void EnterToMatchMaking(ButtonClickEvent e, SelectedMatchMakingModeNode matchMakingModeNode, [JoinAll] SingleNode<MatchMakingComponent> matchMaking,
            [JoinAll] SingleNode<SelfUserComponent> user) {
            if (!matchMakingModeNode.gameModeSelectButton.Restricted) {
                bool flag = user.Entity.HasComponent<SquadGroupComponent>();
                bool flag2 = user.Entity.HasComponent<SquadLeaderComponent>();

                if (!flag || flag2) {
                    RequestEnterToMatchMaking(user.Entity, matchMakingModeNode.Entity);
                }
            }
        }

        [OnEventComplete]
        public void EnterToMatchMaking(PlayAgainEvent e, Node any, [JoinAll] SingleNode<MatchMakingComponent> matchMaking, [JoinAll] SingleNode<SelfUserComponent> user) {
            if (e.ModeIsAvailable) {
                RequestEnterToMatchMaking(user.Entity, e.MatchMackingMode);
            }
        }

        bool RequestEnterToMatchMaking(Entity user, Entity mode) {
            if (user.HasComponent<UserEnteringToMatchMakingComponent>()) {
                return false;
            }

            user.AddComponent<UserEnteringToMatchMakingComponent>();
            ScheduleEvent(new UserEnterToMatchMakingEvent(), mode);
            return true;
        }

        [OnEventFire]
        public void EnterToMatchMaking(UserEnterToMatchMakingEvent e, SingleNode<MatchMakingDefaultModeComponent> mode, [JoinAll] SelfUserNotInSquadNode user) {
            ScheduleEvent(new EnterToMatchMakingEvent(), mode);
        }

        [OnEventFire]
        public void EnterToMatchMaking(UserEnterToMatchMakingEvent e, NotDefaultMatchMakingModeNode mode, [JoinAll] SelfUserNotInSquadNode user) {
            ScheduleEvent(new EnterToMatchMakingEvent(), mode);
        }

        [OnEventFire]
        public void EnteredToMatchMakingEvent(EnteredToMatchMakingEvent e, SingleNode<MatchMakingModeComponent> mode) {
            ScheduleEvent<SaveBattleModeEvent>(mode);
        }

        [OnEventFire]
        public void EnterToMatchMaking(UserEnterToMatchMakingEvent e, SelectedMatchMakingModeNode mode, [JoinAll] SelfUserInSquadNode user) {
            if (user.Entity.HasComponent<SquadLeaderComponent>()) {
                List<long> list = (from sm in Select<SquadUserNode>(user.Entity, typeof(SquadGroupComponent))
                                   select sm.Entity.Id).ToList();

                MainScreenComponent.Instance.lastSelectedGameModeId = mode.Entity;

                ScheduleEvent(new SquadTryEnterToMatchMakingEvent {
                    MatchMakingModeId = mode.Entity.Id,
                    RatingMatchMakingMode = mode.Entity.HasComponent<MatchMakingRatingModeComponent>()
                }, user);
            }
        }

        [OnEventFire]
        public void EnterToMatchMaking(ButtonClickEvent e, SingleNode<StartSquadBattleButtonComponent> button, [JoinAll] SelfUserInSquadNode user) {
            ScheduleEvent(new SquadTryEnterToMatchMakingAfterEnergySharingEvent {
                MatchMakingModeId = MainScreenComponent.Instance.lastSelectedGameModeId.Id
            }, user);
        }

        [OnEventFire]
        public void EnteredToMatchMakingEvent(EnteredToMatchMakingEvent e, Node any, [JoinAll] SingleNode<ShareEnergyScreenComponent> shareScreen) {
            MainScreenComponent.Instance.HideShareEnergyScreen();
        }

        [OnEventFire]
        public void ExitFromSharingDialog(ButtonClickEvent e, SingleNode<CancelEnergySharingButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] SelfUserInSquadNode user) {
            ScheduleEvent<CancelEnergySharingEvent>(user);
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
        }

        [OnEventFire]
        public void ShowShareEnergyWindow(NodeAddedEvent e, SquadInEnergySharingStateNode squad) {
            MainScreenComponent.Instance.ShowShareEnergyScreen();
        }

        [OnEventFire]
        public void HideShareEnergyWindow(NodeRemoveEvent e, SquadInEnergySharingStateNode squad, [JoinAll] SelfUserNode user) {
            MainScreenComponent.Instance.HideShareEnergyScreen();
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
        }

        [OnEventFire]
        public void ShowCustomModesWindow(ButtonClickEvent e, SingleNode<ShowCustomModesScreenButtonComponent> button) {
            MainScreenComponent.Instance.ShowCustomBattleScreen();
        }

        [OnEventFire]
        public void ShowCreateBattle(ButtonClickEvent e, CustomGameModeButtonNode modeButton, [JoinAll] SelfUserNode user, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            if (user.Entity.HasComponent<SquadGroupComponent>() && !user.Entity.HasComponent<SquadLeaderComponent>()) {
                CantStartGameInSquadDialogComponent cantStartGameInSquadDialogComponent = dialogs.component.Get<CantStartGameInSquadDialogComponent>();
                cantStartGameInSquadDialogComponent.CantSearch = false;
                cantStartGameInSquadDialogComponent.Show();
            } else {
                MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.CreateBattle, false);
            }
        }

        [OnEventFire]
        public void EnterFailed(EnterToMatchMakingFailedEvent e, SingleNode<MatchMakingModeComponent> mode, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<BattleLobbyEnterToBattleErrorEvent>(user);
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
        }

        [OnEventFire]
        public void ExitFromMatchMaking(ButtonClickEvent e, SingleNode<CancelMatchSearchingButtonComponent> battleSelect, [JoinAll] SingleNode<MatchMakingComponent> matchMaking) {
            ScheduleEvent(new ExitFromMatchMakingEvent(), matchMaking);
        }

        [OnEventFire]
        public void ExitFromMatchMaking(CancelMatchSearchingEvent e, Node any, [JoinAll] SingleNode<MatchMakingComponent> matchMaking) {
            ScheduleEvent(new ExitFromMatchMakingEvent(), matchMaking);
        }

        [OnEventFire]
        public void ExitFromLobby(ButtonClickEvent e, SingleNode<ExitBattleLobbyButtonComponent> exitButton, [JoinAll] SingleNode<MatchMakingComponent> matchMaking) {
            ScheduleEvent(new ExitFromMatchMakingEvent(), matchMaking);
        }

        public class SelectedMatchMakingModeNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GameModeSelectButtonComponent gameModeSelectButton;

            public MatchMakingModeComponent matchMakingMode;
        }

        [Not(typeof(MatchMakingDefaultModeComponent))]
        public class NotDefaultMatchMakingModeNode : Node {
            public MatchMakingModeComponent matchMakingMode;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SquadGroupComponent))]
        public class SelfUserNotInSquadNode : SelfUserNode { }

        public class SelfUserInSquadNode : SelfUserNode {
            public SquadGroupComponent squadGroup;
        }

        public class SquadUserNode : Node {
            public SquadGroupComponent squadGroup;
            public UserComponent user;
        }

        public class SquadNode : Node {
            public SquadComponent squad;

            public SquadGroupComponent squadGroup;
        }

        public class SquadInEnergySharingStateNode : SquadNode {
            public EnergySharingStateComponent energySharingState;
        }

        public class CustomGameModeButtonNode : Node {
            public ButtonMappingComponent buttonMapping;

            public CustomGameModeComponent customGameMode;
        }
    }
}