using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanHitSoundsEffectComponent : AbstractVulcanSoundEffectComponent {
        [SerializeField] AudioClip staticHitClip;

        [SerializeField] AudioClip targetHitClip;

        public bool IsStaticHit { get; set; }

        public AudioClip StaticHitClip {
            get => staticHitClip;
            set => staticHitClip = value;
        }

        public AudioClip TargetHitClip {
            get => targetHitClip;
            set => targetHitClip = value;
        }
    }
}