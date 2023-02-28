using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class FriendsUITableViewCell : UITableViewCell {
        [SerializeField] UserListItemComponent friendsListItem;

        public long id => friendsListItem.userId;

        public void Init(long userId, bool delayedLoading) {
            friendsListItem.ResetItem(userId, delayedLoading);
        }
    }
}