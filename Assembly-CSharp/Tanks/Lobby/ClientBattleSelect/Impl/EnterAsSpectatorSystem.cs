using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientFriends.Impl;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class EnterAsSpectatorSystem : ECSSystem {
        [OnEventFire]
        public void EnterAsSpectator(ButtonClickEvent e, SingleNode<EnterBattleAsSpectatorButtonComponent> button, [JoinAll] SelectedFriendUI friendUI, [JoinByUser] FriendInBattle friend,
            [JoinAll] ClientSessionNode session, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser) {
            if (!selfBattleLobbyUser.IsPresent()) {
                ScheduleEvent(new EnterBattleAsSpectatorFromLobbyRequestEvent(friend.battleGroup.Key), session);
            }
        }

        [OnEventFire]
        public void EnterAsSpectator(ButtonClickEvent e, SingleNode<EnterBattleAsSpectatorButtonComponent> button, [JoinAll] ProfileScreenWithUserGroupNode profileScreen,
            [JoinByUser] UserInBattleNode userInBattle, [JoinAll] ClientSessionNode session, [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser) {
            if (!selfBattleLobbyUser.IsPresent()) {
                ScheduleEvent(new EnterBattleAsSpectatorFromLobbyRequestEvent(userInBattle.battleGroup.Key), session);
            }
        }

        [OnEventFire]
        public void EnterAsSpectator(EnterAsSpectatorToFriendBattleEvent e, FriendInBattle friend, [JoinAll] ClientSessionNode session,
            [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser) {
            if (!selfBattleLobbyUser.IsPresent()) {
                ScheduleEvent(new EnterBattleAsSpectatorFromLobbyRequestEvent(friend.battleGroup.Key), session);
            }
        }

        [OnEventFire]
        public void ProfileScreenLoadedWithUserInBattle(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserInBattleNode userInBattle,
            [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser) {
            if (!selfBattleLobbyUser.IsPresent()) {
                ShowSpectatorButton(profileScreen);
            }
        }

        [OnEventFire]
        public void UserInBattle(NodeAddedEvent e, UserInBattleNode userInBattle, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen,
            [JoinAll] Optional<SelfBattleLobbyUser> selfBattleLobbyUser) {
            if (!selfBattleLobbyUser.IsPresent()) {
                ShowSpectatorButton(profileScreen);
            }
        }

        [OnEventFire]
        public void UserOutBattle(NodeRemoveEvent e, UserInBattleNode userInBattle, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen) {
            HideSpectatorButton(profileScreen);
        }

        [OnEventFire]
        public void SelfUserInLobby(NodeAddedEvent e, SelfBattleLobbyUser selfBattleLobbyUser, [JoinAll] ProfileScreenWithUserGroupNode profileScreen) {
            HideSpectatorButton(profileScreen);
        }

        void ShowSpectatorButton(ProfileScreenWithUserGroupNode profileScreen) {
            profileScreen.profileScreen.EnterBattleAsSpectatorRow.SetActive(true);
        }

        void HideSpectatorButton(ProfileScreenWithUserGroupNode profileScreen) {
            profileScreen.profileScreen.EnterBattleAsSpectatorRow.SetActive(false);
        }

        public class ClientSessionNode : Node {
            public ClientSessionComponent clientSession;

            public UserGroupComponent userGroup;
        }

        public class SelectedFriendUI : Node {
            public FriendsListItemComponent friendsListItem;
            public SelectedListItemComponent selectedListItem;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class FriendInBattle : Node {
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
        }

        public class ProfileScreenWithUserGroupNode : Node {
            public ActiveScreenComponent activeScreen;
            public ProfileScreenComponent profileScreen;

            public UserGroupComponent userGroup;
        }

        public class UserInBattleNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public BattleGroupComponent battleGroup;

            public UserGroupComponent userGroup;
        }

        public class SelfBattleLobbyUser : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;

            public SelfUserComponent selfUser;
            public UserComponent user;
        }
    }
}