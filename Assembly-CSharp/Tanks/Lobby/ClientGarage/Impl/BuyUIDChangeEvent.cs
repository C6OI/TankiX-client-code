using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1474537061794L)]
    public class BuyUIDChangeEvent : Event {
        public string Uid { get; set; }

        public long Price { get; set; }
    }
}