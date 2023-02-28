using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RicochetBounceSoundEffectComponent : BaseRicochetSoundEffectComponent {
        const float PLAY_INTERVAL_TIME = 0.5f;

        [SerializeField] AudioClip[] avaibleClips;

        int clipIndex;

        int clipsCount;

        float playInterval;

        void Awake() {
            playInterval = -1f;
            enabled = false;
            clipIndex = 0;
            clipsCount = avaibleClips.Length;
        }

        void Update() {
            playInterval -= Time.deltaTime;

            if (playInterval <= 0f) {
                playInterval = -1f;
                enabled = false;
            }
        }

        public override void PlayEffect(Vector3 position) {
            if (!(playInterval > 0f)) {
                base.PlayEffect(position);
                playInterval = 0.5f;
                enabled = true;
            }
        }

        public override void Play(AudioSource sourceInstance) {
            AudioClip clip = avaibleClips[clipIndex];
            sourceInstance.clip = clip;
            clipIndex++;

            if (clipIndex == clipsCount) {
                clipIndex = 0;
            }

            sourceInstance.Play();
        }
    }
}