using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1438590245672L)]
    public class RequestRegisterUserEvent : Event {
        public string Uid { get; set; }

        public string EncryptedPasswordDigest { get; set; }

        public string Email { get; set; }

        public string HardwareFingerprint { get; set; }

        public bool Subscribed { get; set; }

        public bool Steam { get; set; }

        public bool QuickRegistration { get; set; }
    }
}