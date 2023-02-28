using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MatchLobbySoundSystem : ECSSystem {
        [OnEventFire]
        public void PlayLobbySound(NodeAddedEvent e, SelfUserInMatchMakingLobby user, [Context] [JoinByBattleLobby] BattleLobbyNode battleLobby,
            [JoinAll] SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<HangarMatchLobbySoundComponent> hangar) {
            hangar.component.Play();
        }

        public class SelfUserInMatchMakingLobby : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;
            public MatchMakingUserComponent matchMakingUser;

            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class BattleLobbyNode : Node {
            public BattleLobbyComponent battleLobby;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }
    }
}