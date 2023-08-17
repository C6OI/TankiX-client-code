using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.API {
    [SerialVersionUID(1455968728337L)]
    public interface GoodsTemplate : Template {
        [AutoAdded]
        GoodsComponent Goods();
    }
}