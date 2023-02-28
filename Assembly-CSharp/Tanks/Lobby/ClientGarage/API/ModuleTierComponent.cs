using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636330378478033958L)]
    public class ModuleTierComponent : Component {
        public int TierNumber { get; set; }
    }
}