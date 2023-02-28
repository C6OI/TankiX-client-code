using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1479807693001L)]
    public class UserItemCounterComponent : Component {
        public long Count { get; set; }
    }
}