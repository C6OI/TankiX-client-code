using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class LimitedInstancingSoundEffectBehaviour : MonoBehaviour {
        static readonly double[] LAST_PLAY_TIMES = new double[2] { -1.0, -1.0 };

        [SerializeField] AudioSource source;

        [SerializeField] float playDelay = -1f;

        void Update() {
            if (!source.isPlaying) {
                DestroyObject(gameObject);
            }
        }

        static bool CanInstantiateSoundEffect(int index, float minTimeForPlayingSec) {
            if (LAST_PLAY_TIMES[index] < 0.0) {
                return true;
            }

            if (AudioSettings.dspTime - LAST_PLAY_TIMES[index] < minTimeForPlayingSec) {
                return false;
            }

            return true;
        }

        protected static bool CreateSoundEffectInstance(LimitedInstancingSoundEffectBehaviour effectPrefab, int index, float minTimeForPlayingSec) {
            if (!CanInstantiateSoundEffect(index, minTimeForPlayingSec)) {
                return false;
            }

            InstantiateAndPlaySoundEffectInstance(effectPrefab, index);
            return true;
        }

        static void InstantiateAndPlaySoundEffectInstance(LimitedInstancingSoundEffectBehaviour effectPrefab, int index) {
            LimitedInstancingSoundEffectBehaviour limitedInstancingSoundEffectBehaviour = Instantiate(effectPrefab);
            DontDestroyOnLoad(limitedInstancingSoundEffectBehaviour.gameObject);
            limitedInstancingSoundEffectBehaviour.Play(index);
        }

        void Play(int index) {
            if (playDelay <= 0f) {
                source.Play();
                LAST_PLAY_TIMES[index] = AudioSettings.dspTime;
                DestroyObject(gameObject, source.clip.length);
            } else {
                double num = AudioSettings.dspTime + playDelay;
                source.PlayScheduled(num);
                LAST_PLAY_TIMES[index] = num;
                enabled = true;
            }
        }
    }
}