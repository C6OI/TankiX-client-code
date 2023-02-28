using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNotifications.API {
    public class NewPaintItemNotificationTextComponent : Component {
        public string CoverText { get; set; }

        public string PaintText { get; set; }
    }
}