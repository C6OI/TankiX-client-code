using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class DistanceAndAngleTargetEvaluatorComponent : Component {
        public float AngleWeight { get; set; }

        public float DistanceWeight { get; set; }
    }
}