using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636493804557302034L)]
    public interface FullGarageSpecialOfferTemplate : BaseStarterPackSpecialOfferTemplate, SpecialOfferBaseTemplate, GoodsTemplate, Template { }
}