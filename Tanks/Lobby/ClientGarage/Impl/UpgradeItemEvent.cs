using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1436347755482L)]
    public class UpgradeItemEvent : Event {
        public UpgradeItemEvent() { }

        public UpgradeItemEvent(long price) => Price = price;

        public long Price { get; set; }
    }
}