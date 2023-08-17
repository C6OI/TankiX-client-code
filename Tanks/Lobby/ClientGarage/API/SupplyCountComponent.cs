using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437485051427L)]
    [Shared]
    public class SupplyCountComponent : Component {
        public long Count { get; set; }
    }
}