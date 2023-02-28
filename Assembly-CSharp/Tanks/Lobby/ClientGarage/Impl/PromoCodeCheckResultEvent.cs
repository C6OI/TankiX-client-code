using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1490937016798L)]
    public class PromoCodeCheckResultEvent : Event {
        public string Code { get; set; }

        public PromoCodeCheckResult Result { get; set; }
    }
}