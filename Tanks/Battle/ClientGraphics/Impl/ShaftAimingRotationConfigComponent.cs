using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingRotationConfigComponent : MonoBehaviour, Component {
        [SerializeField] float aimingOffsetClipping = 35f;

        [SerializeField] float maxAimingCameraOffset = 30f;

        public float AimingOffsetClipping => aimingOffsetClipping;

        public float MaxAimingCameraOffset => maxAimingCameraOffset;
    }
}