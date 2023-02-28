using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class ConicTargetingComponent : Component {
        public float WorkDistance { get; set; }

        public float HalfConeAngle { get; set; }

        public int HalfConeNumRays { get; set; }

        public int NumSteps { get; set; }

        public float FireOriginOffsetCoeff { get; set; }
    }
}