using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(1516085064117L)]
    public interface BaseStarterPackSpecialOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Template { }
}