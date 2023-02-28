using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1544956558339L)]
    public class RestrictionByUserFractionComponent : Component {
        public long FractionId { get; set; }
    }
}