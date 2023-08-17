using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class CurrentScreenComponent : Component {
        public ShowScreenData showScreenData;

        public Entity screen { get; set; }
    }
}