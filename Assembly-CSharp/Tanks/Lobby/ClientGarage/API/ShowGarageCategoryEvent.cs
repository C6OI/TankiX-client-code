using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ShowGarageCategoryEvent : Event {
        public GarageCategory Category { get; set; }

        public Entity ParentItem { get; set; }

        public Entity SelectedItem { get; set; }

        public Entity ScreenContext { get; set; }
    }
}