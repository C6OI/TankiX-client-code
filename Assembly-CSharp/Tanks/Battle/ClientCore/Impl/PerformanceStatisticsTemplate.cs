using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1448960772759L)]
    public interface PerformanceStatisticsTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        PerformanceStatisticsSettingsComponent performanceStatisticsSettings();

        [AutoAdded]
        PerformanceStatisticsHelperComponent perfomanceStatisticsHelper();
    }
}