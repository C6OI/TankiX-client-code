using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusSoundsBehaviour : MonoBehaviour {
        [SerializeField] AudioSource upgrade;

        [SerializeField] AudioSource click;

        [SerializeField] AudioSource hover;

        [SerializeField] AudioSource take;

        public void PlayUpgrade() {
            Play(upgrade);
        }

        public void PlayClick() {
            Play(click);
        }

        public void PlayHover() {
            Play(hover);
        }

        public void PlayTake() {
            Play(take);
        }

        void Play(AudioSource source) {
            source.Stop();
            source.Play();
        }
    }
}