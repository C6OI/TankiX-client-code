using Platform.Library.ClientDataStructures.API;

namespace Tanks.Battle.ClientCore.Impl {
    public interface FramesStatisticsCollector {
        void Collect(int timeInSec, int maxFrameDurationInMs, Consumer<StatisticCollection> onComplete);

        void Stop();
    }
}