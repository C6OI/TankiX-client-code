using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SkipAutoRemove]
    public class TankCollisionDetectionComponent : Component {
        public TankCollisionDetectionComponent() => Phase = -1L;

        public long Phase { get; set; }

        public TankCollisionDetector Detector { get; set; }
    }
}