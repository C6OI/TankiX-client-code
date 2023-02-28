using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MouseOrbitCameraComponent : Component {
        public float distance = MouseOrbitCameraConstants.DEFAULT_MOUSE_ORBIT_DISTANCE;

        public Quaternion targetRotation { get; set; }
    }
}