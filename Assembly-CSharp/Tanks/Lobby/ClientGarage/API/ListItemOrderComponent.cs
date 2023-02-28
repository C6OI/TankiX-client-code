using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ListItemOrderComponent : Component {
        public ListItemOrderComponent() => Priorities = new ListItemPriorities();

        public ListItemPriorities Priorities { get; set; }
    }
}