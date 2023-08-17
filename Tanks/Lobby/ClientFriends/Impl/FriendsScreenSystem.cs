using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientFriends.API;
using Lobby.ClientFriends.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsScreenSystem : ECSSystem {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void HideAllButtons(NodeRemoveEvent e, SelectedFriendUI friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> screen) => screen.component.BattleButton.SetActive(false);

        [OnEventFire]
        public void ShowBattleButton(NodeAddedEvent e, SelectedFriendUI friendUI,
            [Context] [JoinByUser] FriendInBattle friend, [JoinAll] SingleNode<FriendsScreenComponent> screen) =>
            screen.component.BattleButton.SetActive(true);

        [OnEventFire]
        public void HideBattleButton(NodeRemoveEvent e, FriendInBattle friend, [JoinByUser] SelectedFriendUI friendUI,
            [JoinAll] SingleNode<FriendsScreenComponent> screen) => screen.component.BattleButton.SetActive(false);

        [OnEventFire]
        public void GoToBattleSelectScreen(ButtonClickEvent e, SingleNode<FriendBattleButtonComponent> button,
            [JoinAll] SelectedFriendUI friendUI, [JoinByUser] FriendInBattle friend) =>
            ScheduleEvent(new ShowBattleEvent(friend.battleGroup.Key), EngineService.EntityStub);

        public class SelectedFriendUI : Node {
            public FriendsListItemComponent friendsListItem;
            public SelectedListItemComponent selectedListItem;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class FriendInBattle : Node {
            public AcceptedFriendComponent acceptedFriend;

            public BattleGroupComponent battleGroup;

            public UserGroupComponent userGroup;
        }
    }
}