using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ShowGarageItemEvent : Event {
        public Entity Item { get; set; }
    }
}