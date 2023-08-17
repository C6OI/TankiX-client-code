using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteFriendToBattleNotificationComponent : Component {
        public string MessageTemplate { get; set; }
    }
}