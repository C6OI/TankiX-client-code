using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BulletEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject bulletPrefab;

        [SerializeField] GameObject explosionPrefab;

        [SerializeField] float explosionTime = 1f;

        [SerializeField] float explosionOffset = 0.5f;

        public GameObject BulletPrefab {
            get => bulletPrefab;
            set => bulletPrefab = value;
        }

        public GameObject ExplosionPrefab {
            get => explosionPrefab;
            set => explosionPrefab = value;
        }

        public float ExplosionTime {
            get => explosionTime;
            set => explosionTime = value;
        }

        public float ExplosionOffset {
            get => explosionOffset;
            set => explosionOffset = value;
        }
    }
}