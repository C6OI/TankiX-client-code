using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SelectedItemComponent : Component {
        public SelectedItemComponent() { }

        public SelectedItemComponent(Entity selectedItem) => SelectedItem = selectedItem;

        public Entity SelectedItem { get; set; }
    }
}