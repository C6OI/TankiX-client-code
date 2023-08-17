using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class KillTankSoundEffectBehaviour : MonoBehaviour {
        [SerializeField] AudioSource source;

        [SerializeField] float playDelay = -1f;

        void Update() {
            if (!source.isPlaying) {
                DestroyObject(gameObject);
            }
        }

        public void Play() {
            if (playDelay <= 0f) {
                source.Play();
            } else {
                source.PlayScheduled(AudioSettings.dspTime + playDelay);
            }

            enabled = true;
        }
    }
}