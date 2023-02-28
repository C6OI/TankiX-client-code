using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1439270018242L)]
    public class RegistrationDateComponent : Component {
        [ProtocolOptional] public Date RegistrationDate { get; set; }
    }
}