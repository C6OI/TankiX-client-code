using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RicochetBounceSoundEffectComponent : BaseRicochetSoundEffectComponent {
        [SerializeField] AudioClip[] avaibleClips;

        int clipIndex;

        int clipsCount;

        void Awake() {
            clipIndex = 0;
            clipsCount = avaibleClips.Length;
        }

        public override void Play(AudioSource sourceInstane) {
            AudioClip clip = avaibleClips[clipIndex];
            sourceInstane.clip = clip;
            clipIndex++;

            if (clipIndex == clipsCount) {
                clipIndex = 0;
            }

            sourceInstane.Play();
        }
    }
}