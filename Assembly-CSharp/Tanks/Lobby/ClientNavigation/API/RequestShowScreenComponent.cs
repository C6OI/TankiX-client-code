using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public class RequestShowScreenComponent : Component {
        public ShowScreenEvent Event { get; set; }
    }
}