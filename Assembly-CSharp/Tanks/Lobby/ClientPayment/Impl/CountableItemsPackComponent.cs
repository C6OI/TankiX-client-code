using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1507791699587L)]
    public class CountableItemsPackComponent : Component {
        public Dictionary<long, int> Pack { get; set; }
    }
}