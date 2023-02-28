using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNotifications.API {
    public class IgnoreBattleResultScreenNotificationComponent : Component {
        public int ScreenPartIndex { get; set; }
    }
}