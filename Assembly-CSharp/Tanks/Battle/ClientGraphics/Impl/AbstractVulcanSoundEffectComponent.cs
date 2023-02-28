using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class AbstractVulcanSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectPrefab;

        [SerializeField] float startTimePerSec;

        [SerializeField] float delayPerSec;

        public float DelayPerSec {
            get => delayPerSec;
            set => delayPerSec = value;
        }

        public AudioSource SoundSource { get; set; }

        public GameObject EffectPrefab {
            get => effectPrefab;
            set => effectPrefab = value;
        }

        public float StartTimePerSec {
            get => startTimePerSec;
            set => startTimePerSec = value;
        }
    }
}