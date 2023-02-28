using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636177019617146882L)]
    public class SpecialOfferDurationComponent : Component {
        public bool OneShot { get; set; }
    }
}