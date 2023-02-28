using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [SerialVersionUID(1518597062026L)]
    public interface EnergyCompensationPersistentNotificationTemplate : IgnoreBattleScreenNotificationTemplate, LockScreenNotificationTemplate, NotificationTemplate, Template { }
}