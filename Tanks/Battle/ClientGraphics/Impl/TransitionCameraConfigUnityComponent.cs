using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TransitionCameraConfigUnityComponent : MonoBehaviour, Component {
        [SerializeField] float flyHeight = 1f;

        [SerializeField] float flyTimeSec = 0.55f;

        public float FlyHeight {
            get => flyHeight;
            set => flyHeight = value;
        }

        public float FlyTimeSec {
            get => flyTimeSec;
            set => flyTimeSec = value;
        }
    }
}