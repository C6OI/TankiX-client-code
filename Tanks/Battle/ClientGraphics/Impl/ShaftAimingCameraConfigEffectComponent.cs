using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingCameraConfigEffectComponent : MonoBehaviour, Component {
        [SerializeField] float activationStateTargetFOV = 40f;

        [SerializeField] float workingStateMinFOV = 23f;

        [SerializeField] float recoveringFOVSpeed = 30f;

        public float RecoveringFovSpeed {
            get => recoveringFOVSpeed;
            set => recoveringFOVSpeed = value;
        }

        public float ActivationStateTargetFov {
            get => activationStateTargetFOV;
            set => activationStateTargetFOV = value;
        }

        public float WorkingStateMinFov {
            get => workingStateMinFOV;
            set => workingStateMinFOV = value;
        }
    }
}