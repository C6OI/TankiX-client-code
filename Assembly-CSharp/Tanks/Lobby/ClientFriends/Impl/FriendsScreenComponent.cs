using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsScreenComponent : BehaviourComponent {
        const int actionsForShowButton = 5;

        [SerializeField] FriendsListUIComponent friendsList;

        int acceptFriendsCounter;

        int rejectFriendsCounter;

        public void Show() {
            friendsList.Show();
            acceptFriendsCounter = 0;
            rejectFriendsCounter = 0;
        }

        public void Hide() {
            friendsList.Hide();
        }

        public void HideImmediate() {
            friendsList.HideImmediate();
        }

        public void RemoveUser(long userId, bool toRight) {
            friendsList.RemoveItem(userId, toRight);
        }

        public void AddItem(long userId, string userUid, FriendType friendType) {
            friendsList.AddItem(userId, userUid, friendType);
        }

        public void ResetButtons() {
            friendsList.ResetButtons();
        }

        public void AcceptFriend() {
            rejectFriendsCounter = 0;
            acceptFriendsCounter++;

            if (acceptFriendsCounter >= 5) {
                friendsList.EnableAddAllButton();
            }
        }

        public void RejectFriend() {
            acceptFriendsCounter = 0;
            rejectFriendsCounter++;

            if (rejectFriendsCounter >= 5) {
                friendsList.EnableRejectAllButton();
            }
        }

        public void ClearIncoming(bool moveToAccepted) {
            friendsList.ClearIncoming(moveToAccepted);
        }
    }
}