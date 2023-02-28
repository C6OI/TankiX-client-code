using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class SpecialOfferEndTimeComponent : Component {
        public Date EndDate { get; set; }
    }
}