using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1510171792604L)]
    public class BattleEntrancePayerComponent : Component {
        public Dictionary<long, long> EnergyPayments { get; set; }
    }
}