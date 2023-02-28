using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RPMVolumeUpdaterFinishBehaviour : MonoBehaviour {
        const float SOUND_PAUSE_LATENCY_SEC = 2f;

        [SerializeField] AudioSource source;

        [SerializeField] float soundPauseTimer;

        void Awake() {
            enabled = false;
        }

        void Update() {
            soundPauseTimer -= Time.deltaTime;

            if (soundPauseTimer <= 0f) {
                source.Pause();
                enabled = false;
            }
        }

        void OnEnable() {
            soundPauseTimer = 2f;
        }

        public void Build(AudioSource source) {
            this.source = source;
        }
    }
}