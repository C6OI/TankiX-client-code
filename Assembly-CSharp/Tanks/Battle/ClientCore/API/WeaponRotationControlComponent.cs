using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class WeaponRotationControlComponent : Component {
        public float Control { get; set; }

        public bool CenteringControl { get; set; }

        public float EffectiveControl { get; set; }

        public bool Centering { get; set; }

        public float PrevEffectiveControl { get; set; }

        public float PrevDeltaRotaion { get; set; }

        public float Rotation { get; set; }

        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public float MouseRotationCumulativeHorizontalAngle { get; set; }

        public float MouseRotationCumulativeVerticalAngle { get; set; }

        public int ShaftElevationDirectionByKeyboard { get; set; }

        public double PrevControlChangedTime { get; set; }

        public float MouseShaftAimCumulativeVerticalAngle { get; set; }

        public bool ForceGyroscopeEnabled { get; set; }

        public bool BlockRotate { get; set; }

        public bool IsRotating() => EffectiveControl != 0f;
    }
}