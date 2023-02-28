using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636268029206734486L)]
    public class SteamBuyGoodsEvent : ProcessPaymentEvent { }
}