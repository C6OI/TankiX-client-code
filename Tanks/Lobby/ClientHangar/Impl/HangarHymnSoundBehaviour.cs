using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarHymnSoundBehaviour : MonoBehaviour {
        public static float FILTER_VOLUME;

        [SerializeField] HangarHymnSoundFilter introFilter;

        [SerializeField] HangarHymnSoundFilter loopFilter;

        void Awake() => FILTER_VOLUME = 0f;

        public void Play(bool playWithNitro) {
            if (playWithNitro) {
                introFilter.Play();
                loopFilter.Play(introFilter.Source.clip.length);
            } else {
                loopFilter.Play();
            }
        }

        public void Stop() {
            introFilter.Stop();
            loopFilter.Stop();
        }
    }
}