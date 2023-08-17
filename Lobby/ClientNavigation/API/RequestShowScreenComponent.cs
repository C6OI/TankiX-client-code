using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class RequestShowScreenComponent : Component {
        public ShowScreenEvent Event { get; set; }
    }
}