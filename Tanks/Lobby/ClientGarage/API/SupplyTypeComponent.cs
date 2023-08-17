using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(-1718386085766502073L)]
    public class SupplyTypeComponent : Component {
        public SupplyType Type { get; set; }
    }
}