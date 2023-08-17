using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class KalmanFilterComponent : Component {
        public KalmanFilter kalmanFilterPosition;

        public float kalmanUpdatePeriod = 0.033f;

        public float kalmanUpdateTimeAccumulator;
    }
}