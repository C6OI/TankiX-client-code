using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class InviteFriendsPopupSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void ShowInvitePopup(ButtonClickEvent e, SingleNode<InviteFriendsButtonComponent> button, [JoinAll] SingleNode<InviteFriendsPopupComponent> friendsPopup) {
            friendsPopup.component.ShowInvite(button.component.PopupPosition, new Vector2(0.5f, 0f), InviteMode.Lobby);
        }

        [OnEventFire]
        public void OnItemClick(UserLabelClickEvent e, UserLabelNode userLabel, [JoinAll] SingleNode<InviteFriendsPopupComponent> friendsPopup) { }

        [OnEventFire]
        public void RequestSortedFriends(NodeAddedEvent e, InviteFriendsUINode inviteFriendsUi, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent<LoadSortedFriendsIdsWithNicknamesEvent>(session);
        }

        [OnEventFire]
        public void CreateUIsForFriends(SortedFriendsIdsWithNicknamesLoaded e, SingleNode<ClientSessionComponent> session, [JoinAll] InviteFriendsUINode inviteFriendsUi) {
            inviteFriendsUi.inviteFriendsUi.AddFriends(e.FriendsIdsAndNicknames, FriendType.None);
        }

        public class InviteFriendsUINode : Node {
            public InviteFriendsUIComponent inviteFriendsUi;
        }

        public class FriendsStorageNode : Node {
            public FriendsComponent friends;
        }

        public class UserLabelNode : Node {
            public UserLabelComponent userLabel;
        }
    }
}