using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(1437480091995L)]
    [Shared]
    public class LoginByPasswordEvent : Event {
        public string PasswordEncipher { get; set; }

        public bool RememberMe { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}