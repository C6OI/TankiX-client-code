using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public class NavigateLinkEvent : Event {
        public string Link { get; set; }
    }
}