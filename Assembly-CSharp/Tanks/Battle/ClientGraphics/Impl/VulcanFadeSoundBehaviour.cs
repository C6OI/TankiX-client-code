using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(AudioSource))]
    public class VulcanFadeSoundBehaviour : MonoBehaviour {
        public float fadeDuration;

        public float maxVolume;

        float fadeSpeed;
        AudioSource source;

        void FixedUpdate() {
            source.volume += fadeSpeed * Time.fixedDeltaTime;

            if (!source.isPlaying) {
                enabled = false;
            }
        }

        void OnEnable() {
            source = gameObject.GetComponent<AudioSource>();
            source.volume = maxVolume;
            fadeSpeed = (0f - maxVolume) / fadeDuration;
        }
    }
}