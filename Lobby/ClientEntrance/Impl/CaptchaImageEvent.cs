using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1438162132677L)]
    public class CaptchaImageEvent : Event {
        public byte[] CaptchaBytes { get; set; }
    }
}