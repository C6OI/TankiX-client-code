using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CheckForQuickGameEvent : Event {
        public bool IsQuickGame { get; set; }
    }
}