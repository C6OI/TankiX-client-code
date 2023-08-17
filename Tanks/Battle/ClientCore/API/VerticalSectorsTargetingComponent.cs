using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class VerticalSectorsTargetingComponent : Component {
        public float WorkDistance { get; set; }

        public float VAngleUp { get; set; }

        public float VAngleDown { get; set; }

        public float HAngle { get; set; }

        public float RaysPerDegree { get; set; }
    }
}