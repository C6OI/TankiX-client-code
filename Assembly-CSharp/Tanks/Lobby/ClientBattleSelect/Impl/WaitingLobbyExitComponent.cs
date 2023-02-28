using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class WaitingLobbyExitComponent : Component {
        public AcceptInviteEvent AcceptInviteEvent { get; set; }
    }
}