using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1490877430206L)]
    public class ActivatePromoCodeEvent : Event {
        public string Code { get; set; }
    }
}