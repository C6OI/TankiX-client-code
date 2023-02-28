using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1460403525230L)]
    public class RequestChangePasswordEvent : Event {
        public string PasswordDigest { get; set; }

        public string HardwareFingerprint { get; set; }
    }
}