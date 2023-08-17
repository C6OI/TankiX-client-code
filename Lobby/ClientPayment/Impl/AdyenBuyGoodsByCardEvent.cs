using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1454505010104L)]
    public class AdyenBuyGoodsByCardEvent : Event {
        public string EncrypedCard { get; set; }
    }
}