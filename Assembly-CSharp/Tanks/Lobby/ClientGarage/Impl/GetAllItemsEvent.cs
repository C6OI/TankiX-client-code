using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GetAllItemsEvent<T> : Event where T : GarageItem {
        public GetAllItemsEvent() => Items = new List<T>();

        public List<T> Items { get; set; }
    }
}