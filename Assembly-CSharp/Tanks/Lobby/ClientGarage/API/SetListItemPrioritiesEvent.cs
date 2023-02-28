using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class SetListItemPrioritiesEvent : Event {
        public SetListItemPrioritiesEvent() => Priorities = new ListItemPriorities();

        public ListItemPriorities Priorities { get; set; }
    }
}