using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNotifications.API {
    public class NewItemNotificationTextComponent : Component {
        public string HeaderText { get; set; }

        public string ItemText { get; set; }

        public string SingleItemText { get; set; }
    }
}