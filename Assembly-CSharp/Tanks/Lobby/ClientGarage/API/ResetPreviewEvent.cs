using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ResetPreviewEvent : Event {
        public long ExceptPreviewGroup { get; set; }
    }
}