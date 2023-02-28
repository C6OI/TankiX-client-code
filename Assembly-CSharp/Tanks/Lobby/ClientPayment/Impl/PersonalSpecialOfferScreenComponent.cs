using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1508416389874L)]
    public class PersonalSpecialOfferScreenComponent : SharedChangeableComponent {
        public bool Shown { get; set; }

        public bool ShownWhenRemains { get; set; }
    }
}