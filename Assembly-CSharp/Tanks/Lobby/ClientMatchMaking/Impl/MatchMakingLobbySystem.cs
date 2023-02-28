using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientMatchMaking.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class MatchMakingLobbySystem : ECSSystem {
        [OnEventFire]
        public void cleanUpOnEnterBattle(NodeAddedEvent e, SelfUserInBattle user, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby) {
            cleanUpLobby(lobby.Entity);
        }

        [OnEventFire]
        public void setTimeOut(MatchMakingLobbyStartTimeEvent e, SelfUserInMatchMakingLobby user, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby) {
            cleanUpLobby(lobby.Entity);
            ClientMatchMakingLobbyStartTimeComponent clientMatchMakingLobbyStartTimeComponent = new();
            clientMatchMakingLobbyStartTimeComponent.StartTime = e.StartTime;
            lobby.Entity.AddComponent(clientMatchMakingLobbyStartTimeComponent);
        }

        [OnEventFire]
        public void setStarting(MatchMakingLobbyStartingEvent e, SelfUserInMatchMakingLobby user, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby) {
            cleanUpLobby(lobby.Entity);
            ClientMatchMakingLobbyStartingComponent component = new();
            lobby.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void ExitOnInviteToLobbyConfirm(DialogConfirmEvent e, SingleNode<InviteToLobbyDialogComponent> dialog, [JoinAll] SingleNode<SelfUserComponent> user,
            [JoinByUser] Optional<SingleNode<BattleUserComponent>> battleUser, [JoinAll] SingleNode<MatchMakingComponent> matchMaking) {
            ScheduleEvent(new ExitFromMatchMakingEvent {
                InBattle = battleUser.IsPresent()
            }, matchMaking);
        }

        [OnEventFire]
        public void SoftAdd(NodeAddedEvent e, SingleNode<MatchMakingLobbyStartTimeComponent> lobby) {
            if (!lobby.Entity.HasComponent<ClientMatchMakingLobbyStartTimeComponent>()) {
                lobby.Entity.AddComponent(new ClientMatchMakingLobbyStartTimeComponent {
                    StartTime = lobby.component.StartTime
                });
            }
        }

        [OnEventFire]
        public void SoftRemove(NodeRemoveEvent e, SingleNode<MatchMakingLobbyStartTimeComponent> lobby) {
            if (lobby.Entity.HasComponent<ClientMatchMakingLobbyStartTimeComponent>()) {
                lobby.Entity.RemoveComponent<ClientMatchMakingLobbyStartTimeComponent>();
            }
        }

        [OnEventFire]
        public void SoftAdd(NodeAddedEvent e, SingleNode<MatchMakingLobbyStartingComponent> lobby) {
            if (!lobby.Entity.HasComponent<ClientMatchMakingLobbyStartingComponent>()) {
                lobby.Entity.AddComponent<ClientMatchMakingLobbyStartingComponent>();
            }
        }

        [OnEventFire]
        public void SoftRemove(NodeRemoveEvent e, SingleNode<MatchMakingLobbyStartingComponent> lobby) {
            if (lobby.Entity.HasComponent<ClientMatchMakingLobbyStartingComponent>()) {
                lobby.Entity.RemoveComponent<ClientMatchMakingLobbyStartingComponent>();
            }
        }

        void cleanUpLobby(Entity lobby) {
            if (lobby.HasComponent<ClientMatchMakingLobbyStartTimeComponent>()) {
                lobby.RemoveComponent<ClientMatchMakingLobbyStartTimeComponent>();
            }

            if (lobby.HasComponent<ClientMatchMakingLobbyStartingComponent>()) {
                lobby.RemoveComponent<ClientMatchMakingLobbyStartingComponent>();
            }
        }

        public class SelfUserInMatchMakingLobby : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;
            public MatchMakingUserComponent matchMakingUser;

            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class SelfUserInBattle : SelfUserInMatchMakingLobby {
            public BattleGroupComponent battleGroup;
        }
    }
}