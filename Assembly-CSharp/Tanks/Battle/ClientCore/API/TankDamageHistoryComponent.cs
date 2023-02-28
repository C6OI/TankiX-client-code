using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-6990482179466020052L)]
    public class TankDamageHistoryComponent : Component {
        public List<DamageHistoryItem> DamageHistory { get; set; }
    }
}