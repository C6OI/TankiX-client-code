using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1454585264587L)]
    public interface InviteFriendToBattleNotificationTemplate : NotificationTemplate, Template {
        [PersistentConfig]
        [AutoAdded]
        InviteFriendToBattleNotificationComponent inviteFriendToBattleNotification();
    }
}