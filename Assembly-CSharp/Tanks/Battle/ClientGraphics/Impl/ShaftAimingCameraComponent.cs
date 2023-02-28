using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingCameraComponent : Component {
        public Vector3 WorldInitialCameraPosition { get; set; }

        public Quaternion WorldInitialCameraRotation { get; set; }

        public float InitialFOV { get; set; }
    }
}