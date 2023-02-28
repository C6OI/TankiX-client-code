using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1438075609642L)]
    public class AutoLoginUserEvent : Event {
        public string Uid { get; set; }

        public byte[] EncryptedToken { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}