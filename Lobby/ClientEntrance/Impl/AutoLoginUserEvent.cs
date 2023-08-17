using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(1438075609642L)]
    [Shared]
    public class AutoLoginUserEvent : Event {
        public string Uid { get; set; }

        public byte[] EncryptedToken { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}