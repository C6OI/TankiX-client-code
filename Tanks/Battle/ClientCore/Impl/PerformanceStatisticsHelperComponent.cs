using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class PerformanceStatisticsHelperComponent : Component {
        public FramesCollection frames;
        public float startRoundTimeInSec;

        public StatisticCollection tankCount;
    }
}