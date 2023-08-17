using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BonusDataComponent : Component {
        public float BoxHeight { get; set; }

        public float ParachuteHalfHeight { get; set; }

        public float SwingPivotY { get; set; }

        public Vector3 GroundPoint { get; set; }

        public Vector3 GroundPointNormal { get; set; }

        public Vector3 LandingPoint { get; set; }

        public Vector3 LandingAxis { get; set; }

        public float FallDuration { get; set; }

        public float AlignmentToGroundDuration { get; set; }
    }
}