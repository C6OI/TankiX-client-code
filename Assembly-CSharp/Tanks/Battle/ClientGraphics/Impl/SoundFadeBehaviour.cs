using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundFadeBehaviour : MonoBehaviour {
        [SerializeField] AudioSource source;

        [SerializeField] float fadeOutTime = 1.5f;

        float fadeSpeed;

        void Awake() {
            fadeSpeed = 1f / fadeOutTime;
        }

        void Update() {
            source.volume -= fadeSpeed * Time.deltaTime;
        }
    }
}