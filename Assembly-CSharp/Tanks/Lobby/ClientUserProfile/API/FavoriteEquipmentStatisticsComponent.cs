using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1522236020112L)]
    public class FavoriteEquipmentStatisticsComponent : Component {
        public Dictionary<long, long> HullStatistics { get; set; }

        public Dictionary<long, long> TurretStatistics { get; set; }
    }
}