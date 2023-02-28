using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class CheckForTutorialEvent : Event {
        public bool TutorialIsActive { get; set; }
    }
}