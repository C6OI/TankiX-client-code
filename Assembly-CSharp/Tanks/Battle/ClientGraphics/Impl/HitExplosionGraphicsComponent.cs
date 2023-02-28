using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HitExplosionGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject explosionAsset;

        [SerializeField] float explosionDuration;

        [SerializeField] float explosionOffset;

        [SerializeField] bool useForBlockedWeapon = true;

        public bool UseForBlockedWeapon {
            get => useForBlockedWeapon;
            set => useForBlockedWeapon = value;
        }

        public GameObject ExplosionAsset {
            get => explosionAsset;
            set => explosionAsset = value;
        }

        public float ExplosionDuration {
            get => explosionDuration;
            set => explosionDuration = value;
        }

        public float ExplosionOffset {
            get => explosionOffset;
            set => explosionOffset = value;
        }
    }
}