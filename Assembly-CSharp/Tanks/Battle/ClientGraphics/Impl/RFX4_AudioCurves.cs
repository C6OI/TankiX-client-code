using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RFX4_AudioCurves : MonoBehaviour {
        public AnimationCurve AudioCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public float GraphTimeMultiplier = 1f;

        public bool IsLoop;

        AudioSource audioSource;

        bool canUpdate;

        float startTime;

        float startVolume;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            startVolume = audioSource.volume;
            audioSource.volume = AudioCurve.Evaluate(0f);
        }

        void Update() {
            float num = Time.time - startTime;

            if (canUpdate) {
                float volume = AudioCurve.Evaluate(num / GraphTimeMultiplier) * startVolume;
                audioSource.volume = volume;
            }

            if (num >= GraphTimeMultiplier) {
                if (IsLoop) {
                    startTime = Time.time;
                } else {
                    canUpdate = false;
                }
            }
        }

        void OnEnable() {
            startTime = Time.time;
            canUpdate = true;
        }
    }
}