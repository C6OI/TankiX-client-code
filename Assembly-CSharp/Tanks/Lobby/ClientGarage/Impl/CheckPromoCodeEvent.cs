using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1490931976968L)]
    public class CheckPromoCodeEvent : Event {
        public string Code { get; set; }
    }
}