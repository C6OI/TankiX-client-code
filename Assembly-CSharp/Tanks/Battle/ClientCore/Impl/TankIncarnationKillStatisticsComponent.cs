using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1491549293967L)]
    public class TankIncarnationKillStatisticsComponent : Component {
        public int Kills { get; set; }
    }
}