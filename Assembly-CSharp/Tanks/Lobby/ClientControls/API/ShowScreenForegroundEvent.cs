using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class ShowScreenForegroundEvent : Event {
        public float Alpha { get; set; }
    }
}