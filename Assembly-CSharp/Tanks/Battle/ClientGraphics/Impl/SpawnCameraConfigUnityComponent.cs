using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SpawnCameraConfigUnityComponent : MonoBehaviour, Component {
        [SerializeField] float flyHeight = 30f;

        [SerializeField] float flyTimeSec = 2f;

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