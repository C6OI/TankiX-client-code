using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(636069486689542493L)]
    [Shared]
    public class UidReservationCodeComponent : Component {
        public string Uid { get; set; }

        public string Code { get; set; }
    }
}