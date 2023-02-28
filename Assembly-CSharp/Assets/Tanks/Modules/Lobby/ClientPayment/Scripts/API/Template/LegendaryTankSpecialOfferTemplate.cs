using Lobby.ClientPayment.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Assets.tanks.modules.lobby.ClientPayment.Scripts.API.Template {
    [SerialVersionUID(484234654688075652L)]
    public interface LegendaryTankSpecialOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Platform.Kernel.ECS.ClientEntitySystem.API.Template { }
}