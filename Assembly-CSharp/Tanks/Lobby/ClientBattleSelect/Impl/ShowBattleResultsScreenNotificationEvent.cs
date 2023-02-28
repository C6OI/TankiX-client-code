using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ShowBattleResultsScreenNotificationEvent : Event {
        public int Index { get; set; }

        public bool NotificationExist { get; set; }
    }
}