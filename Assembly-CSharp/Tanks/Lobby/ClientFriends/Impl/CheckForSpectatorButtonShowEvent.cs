using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class CheckForSpectatorButtonShowEvent : Event {
        public bool CanGoToSpectatorMode { get; set; }
    }
}