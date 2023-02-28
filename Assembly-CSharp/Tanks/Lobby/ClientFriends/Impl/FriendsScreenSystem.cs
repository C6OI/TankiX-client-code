using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsScreenSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void ShowFriendsScreen(ButtonClickEvent e, SingleNode<OpenFriendsScreenButtonComponent> node, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            friendsScreen.component.Show();
        }

        [OnEventFire]
        public void HideFriendsScreen(ButtonClickEvent e, SingleNode<HideFriendsScreenButtonComponent> node, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            friendsScreen.component.Hide();
        }

        [OnEventFire]
        public void HideWithMainScreen(NodeRemoveEvent e, SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            friendsScreen.component.HideImmediate();
        }

        [OnEventFire]
        public void OpenAddFriendDialog(ButtonClickEvent e, SingleNode<OpenAddFriendDialogButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            friendsScreen.component.Hide();
            AddFriendDialogComponent addFriendDialogComponent = dialogs.component.Get<AddFriendDialogComponent>();
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            addFriendDialogComponent.Show(animators);
        }

        [OnEventFire]
        public void AcceptFriend(ButtonClickEvent e, SingleNode<AcceptFriendRequestButtonComponent> button, [JoinByUser] IncomingFriendNode incomingFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen, [JoinAll] ICollection<IncomingFriendNode> incomingFriends) {
            ScheduleEvent(new AcceptFriendEvent(incomingFriend.Entity), selfUser);
            friendsScreen.component.RemoveUser(incomingFriend.Entity.Id, false);
            friendsScreen.component.AddItem(incomingFriend.Entity.Id, incomingFriend.userUid.Uid, FriendType.Accepted);

            if (incomingFriends.Count > 1) {
                friendsScreen.component.AcceptFriend();
            } else {
                friendsScreen.component.ResetButtons();
            }
        }

        [OnEventFire]
        public void AcceptAll(ButtonClickEvent e, SingleNode<AcceptAllFriendsButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            ScheduleEvent<AcceptAllFriendsEvent>(selfUser);
            friendsScreen.component.ClearIncoming(true);
            friendsScreen.component.ResetButtons();
        }

        [OnEventFire]
        public void RejectFriend(ButtonClickEvent e, SingleNode<DeclineFriendRequestButtonComponent> button, [JoinByUser] IncomingFriendNode incommingFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen, [JoinAll] ICollection<IncomingFriendNode> incomingFriends) {
            ScheduleEvent(new RejectFriendEvent(incommingFriend.Entity), selfUser);
            friendsScreen.component.RemoveUser(incommingFriend.Entity.Id, true);
            friendsScreen.component.RejectFriend();

            if (incomingFriends.Count > 1) {
                friendsScreen.component.RejectFriend();
            } else {
                friendsScreen.component.ResetButtons();
            }
        }

        [OnEventFire]
        public void RejectAll(ButtonClickEvent e, SingleNode<RejectAllFriendsButtonComponent> button, [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            ScheduleEvent<RejectAllFriendsEvent>(selfUser);
            friendsScreen.component.ClearIncoming(false);
            friendsScreen.component.ResetButtons();
        }

        [OnEventFire]
        public void RevokeFriend(ButtonClickEvent e, SingleNode<RevokeFriendRequestButtonComponent> button, [JoinByUser] OutgoingFriendNode outgoingFriend,
            [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            ScheduleEvent(new RevokeFriendEvent(outgoingFriend.Entity), selfUser);
            friendsScreen.component.RemoveUser(outgoingFriend.Entity.Id, true);
        }

        [OnEventFire]
        public void MoveFriendsScreenToCanvas(NodeAddedEvent e, SingleNode<FriendsScreenComponent> friendsScreen, SingleNode<ScreensLayerComponent> layerNode) {
            GameObject gameObject = friendsScreen.component.transform.parent.gameObject;
            friendsScreen.component.transform.SetParent(layerNode.component.screens60Layer, false);
            Object.Destroy(gameObject);
        }

        [OnEventFire]
        public void HideScreen(ShowProfileScreenEvent e, Node any, [JoinAll] SingleNode<FriendsScreenComponent> friendsScreen) {
            friendsScreen.component.Hide();
        }

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

        public class IncomingFriendNode : Node {
            public IncommingFriendComponent incommingFriend;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class OutgoingFriendNode : Node {
            public OutgoingFriendComponent outgoingFriend;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class FriendLabelNode : Node {
            public IncomingFriendButtonsComponent incomingFriendButtons;

            public OutgoingFriendButtonsComponent outgoingFriendButtons;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;
        }
    }
}