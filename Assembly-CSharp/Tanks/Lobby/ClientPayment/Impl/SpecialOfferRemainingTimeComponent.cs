using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636179208446312959L)]
    public class SpecialOfferRemainingTimeComponent : Component {
        public long Remain { get; set; }

        [ProtocolTransient] public Date EndDate { get; set; }
    }
}