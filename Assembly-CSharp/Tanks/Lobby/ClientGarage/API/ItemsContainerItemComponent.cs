using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1479806073802L)]
    public class ItemsContainerItemComponent : Component {
        public List<ContainerItem> Items { get; set; }

        public List<ContainerItem> RareItems { get; set; }
    }
}