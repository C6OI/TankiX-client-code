using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1438590245672L)]
    [Shared]
    public class RequestRegisterUserEvent : Event {
        public string Uid { get; set; }

        public string EncryptedPasswordDigest { get; set; }

        public string Email { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}