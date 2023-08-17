using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1436432834963L)]
    [Shared]
    public class NextUpgradePriceComponent : Component {
        public long Price { get; set; }
    }
}