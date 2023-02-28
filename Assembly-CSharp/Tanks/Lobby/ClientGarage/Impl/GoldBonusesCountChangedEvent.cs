using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1532516266008L)]
    public class GoldBonusesCountChangedEvent : Event {
        public long NewCount { get; set; }
    }
}