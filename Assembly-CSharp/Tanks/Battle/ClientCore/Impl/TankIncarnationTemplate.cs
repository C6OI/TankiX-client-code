using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1478091203635L)]
    public interface TankIncarnationTemplate : Template {
        TankIncarnationKillStatisticsComponent tankIncarnationKillStatistics();
    }
}