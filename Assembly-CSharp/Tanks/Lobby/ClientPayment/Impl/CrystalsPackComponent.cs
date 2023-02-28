using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1507791399668L)]
    public class CrystalsPackComponent : Component {
        public long Amount { get; set; }

        public long Bonus { get; set; }

        [ProtocolTransient] public long Total => Amount + Bonus;
    }
}