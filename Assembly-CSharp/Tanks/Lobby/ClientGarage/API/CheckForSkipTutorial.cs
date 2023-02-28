using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class CheckForSkipTutorial : Event {
        public bool canSkipTutorial { get; set; }
    }
}