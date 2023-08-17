using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(4186891190183470299L)]
    public class ShaftAimingWorkingStateComponent : TimeValidateComponent {
        public ShaftAimingWorkingStateComponent() {
            VerticalAngle = 0f;
            VerticalSpeed = 0f;
            VerticalElevationDir = 0;
            InitialEnergy = 0f;
            ExhaustedEnergy = 0f;
            WorkingDirection = Vector3.zero;
            IsActive = false;
        }

        public float InitialEnergy { get; set; }

        public float ExhaustedEnergy { get; set; }

        public float VerticalAngle { get; set; }

        public Vector3 WorkingDirection { get; set; }

        public float VerticalSpeed { get; set; }

        public int VerticalElevationDir { get; set; }

        public bool IsActive { get; set; }
    }
}