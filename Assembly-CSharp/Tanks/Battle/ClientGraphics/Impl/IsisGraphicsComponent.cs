using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject rayPrefab;

        public GameObject RayPrefab {
            get => rayPrefab;
            set => rayPrefab = value;
        }

        public IsisRayEffectBehaviour Ray { get; set; }
    }
}