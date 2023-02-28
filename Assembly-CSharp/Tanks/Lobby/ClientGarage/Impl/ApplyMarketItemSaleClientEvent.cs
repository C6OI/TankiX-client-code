using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1526905531643L)]
    public class ApplyMarketItemSaleClientEvent : Event {
        public Date EndDate { get; set; }
    }
}