using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HitExplosionSoundComponent : MonoBehaviour, Component {
        [SerializeField] GameObject soundPrefab;

        [SerializeField] float duration = 2f;

        public float Duration {
            get => duration;
            set => duration = value;
        }

        public GameObject SoundPrefab {
            get => soundPrefab;
            set => soundPrefab = value;
        }
    }
}