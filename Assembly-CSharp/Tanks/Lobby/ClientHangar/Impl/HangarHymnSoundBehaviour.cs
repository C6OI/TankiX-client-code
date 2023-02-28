using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarHymnSoundBehaviour : MonoBehaviour {
        public static float FILTER_VOLUME;

        [SerializeField] HangarHymnSoundFilter introFilter;

        [SerializeField] HangarHymnSoundFilter hangarLoopFilter;

        [SerializeField] HangarHymnSoundFilter battleResultLoopFilter;

        void Awake() {
            FILTER_VOLUME = 0f;
        }

        public void Play(bool playWithNitro) {
            if (playWithNitro) {
                introFilter.Play();
                hangarLoopFilter.Play(introFilter.Source.clip.length);
            } else {
                hangarLoopFilter.Play();
                battleResultLoopFilter.Play();
            }
        }

        public void Stop() {
            introFilter.Stop();
            hangarLoopFilter.Stop();
            battleResultLoopFilter.Stop();
        }
    }
}