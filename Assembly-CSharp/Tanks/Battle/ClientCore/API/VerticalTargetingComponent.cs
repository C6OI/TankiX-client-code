using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class VerticalTargetingComponent : Component {
        public float WorkDistance { get; set; }

        public float AngleUp { get; set; }

        public float AngleDown { get; set; }

        public int NumRaysUp { get; set; }

        public int NumRaysDown { get; set; }
    }
}