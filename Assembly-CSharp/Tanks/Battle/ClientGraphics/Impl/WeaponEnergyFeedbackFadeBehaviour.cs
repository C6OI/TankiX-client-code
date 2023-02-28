using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class WeaponEnergyFeedbackFadeBehaviour : MonoBehaviour {
        const float FADE_OUT_TIME = 0.5f;

        const float FADE_OUT_SPEED = 2f;

        [SerializeField] AudioSource source;

        void Update() {
            source.volume -= 2f * Time.deltaTime;
        }
    }
}