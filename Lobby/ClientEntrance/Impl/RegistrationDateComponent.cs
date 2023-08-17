using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1439270028242L)]
    public class RegistrationDateComponent : Component {
        public long RegistrationDate { get; set; }
    }
}