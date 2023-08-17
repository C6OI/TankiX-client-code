using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(4993925843865197617L)]
    [Shared]
    public class ArmorEffectComponent : Component {
        public float ArmorCoefficient { get; set; }
    }
}