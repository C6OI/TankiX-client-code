using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class NewsItemFilterEvent : Event {
        public bool Hide { get; set; }
    }
}