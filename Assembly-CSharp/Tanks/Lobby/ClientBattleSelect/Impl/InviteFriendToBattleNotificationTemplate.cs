using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1454585264587L)]
    public interface InviteFriendToBattleNotificationTemplate : NotificationTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        InviteFriendToBattleNotificationComponent inviteFriendToBattleNotification();
    }
}