using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteFriendsConfigComponent : Component {
        public float InviteSentNotificationDuration { get; set; }
    }
}