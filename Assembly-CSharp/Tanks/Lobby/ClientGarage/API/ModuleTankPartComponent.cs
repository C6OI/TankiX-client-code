using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636324457894395944L)]
    public class ModuleTankPartComponent : Component {
        public TankPartModuleType TankPart { get; set; }
    }
}