using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API.Energy {
    [SerialVersionUID(636409029061120290L)]
    public interface QuantumMarketItemTemplate : EnergyItemTemplate, MarketItemTemplate, ItemImagedTemplate, GarageItemImagedTemplate, GarageItemTemplate, Template { }
}