using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class BonusConfigComponent : Component {
        public float FallSpeed { get; set; }

        public float AngularSpeed { get; set; }

        public float SwingFreq { get; set; }

        public float SwingAngle { get; set; }

        public float AlignmentToGroundAngularSpeed { get; set; }

        public float AppearingOnGroundTime { get; set; }

        public float SpawnDuration { get; set; }
    }
}