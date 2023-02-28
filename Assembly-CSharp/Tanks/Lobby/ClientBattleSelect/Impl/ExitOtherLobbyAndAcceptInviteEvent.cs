using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ExitOtherLobbyAndAcceptInviteEvent : Event {
        public AcceptInviteEvent AcceptInviteEvent { get; set; }
    }
}