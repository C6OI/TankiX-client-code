using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class PerformanceStatisticsSettingsComponent : Component {
        public int DelayInSecBeforeMeasuringStarted { get; set; }

        public int HugeFrameDurationInMs { get; set; }

        public int MeasuringIntervalInSec { get; set; }
    }
}