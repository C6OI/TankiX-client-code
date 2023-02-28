using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemsListForViewComponent : Component {
        public ItemsListForViewComponent() { }

        public ItemsListForViewComponent(List<Entity> items) => Items = items;

        public List<Entity> Items { get; set; }
    }
}