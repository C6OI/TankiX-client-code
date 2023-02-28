using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1499175516647L)]
    public class KillsEquipmentStatisticsComponent : Component {
        public Dictionary<long, long> HullStatistics { get; set; }

        public Dictionary<long, long> TurretStatistics { get; set; }
    }
}