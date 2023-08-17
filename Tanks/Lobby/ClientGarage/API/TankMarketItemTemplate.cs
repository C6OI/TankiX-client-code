using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433406732656L)]
    public interface TankMarketItemTemplate : Template, GarageItemTemplate, MarketItemTemplate, TankItemTemplate { }
}