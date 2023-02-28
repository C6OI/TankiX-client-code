using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingCameraKickbackComponent : Component {
        public ShaftAimingCameraKickbackComponent(Vector3 lastPosition, Quaternion lastRotation) {
            LastPosition = lastPosition;
            LastRotation = lastRotation;
        }

        public Vector3 LastPosition { get; set; }

        public Quaternion LastRotation { get; set; }
    }
}