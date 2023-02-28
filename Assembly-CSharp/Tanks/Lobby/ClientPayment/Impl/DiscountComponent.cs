using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(643487924561268453L)]
    public class DiscountComponent : Component {
        public float DiscountCoeff { get; set; }
    }
}