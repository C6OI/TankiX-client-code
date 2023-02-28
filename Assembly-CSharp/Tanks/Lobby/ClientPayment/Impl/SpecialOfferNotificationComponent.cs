using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636197700165696728L)]
    public class SpecialOfferNotificationComponent : Component {
        public string OfferName { get; set; }
    }
}