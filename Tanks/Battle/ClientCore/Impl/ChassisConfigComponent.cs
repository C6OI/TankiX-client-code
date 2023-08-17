using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ChassisConfigComponent : Component {
        public bool ReverseBackTurn { get; set; }

        public float TrackSeparation { get; set; }

        public float SuspensionRayOffsetY { get; set; }

        public int NumRaysPerTrack { get; set; }

        public float MaxRayLength { get; set; }

        public float NominalRayLength { get; set; }
    }
}