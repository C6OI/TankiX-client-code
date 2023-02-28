using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientFriends.API {
    [SerialVersionUID(1507711452261L)]
    public interface FriendSentNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        FriendSentNotificationComponent friendSentNotification();
    }
}