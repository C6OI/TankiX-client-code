using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636326081851010949L)]
    public class SlotTankPartComponent : Component {
        public TankPartModuleType TankPart { get; set; }
    }
}