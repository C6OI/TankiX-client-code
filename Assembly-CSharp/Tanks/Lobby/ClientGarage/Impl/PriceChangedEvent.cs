using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1482854686702L)]
    public class PriceChangedEvent : Event {
        public long OldPrice { get; set; }

        public long Price { get; set; }

        public long OldXPrice { get; set; }

        public long XPrice { get; set; }
    }
}