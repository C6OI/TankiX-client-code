using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1437480091995L)]
    public class LoginByPasswordEvent : Event {
        public string PasswordEncipher { get; set; }

        public bool RememberMe { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}