using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public abstract class FadeSoundFilter : MonoBehaviour {
        const float MIN_FADE_TIME_SEC = 0.01f;

        [SerializeField] protected AudioSource source;

        [SerializeField] float fadeInTimeSec = 1f;

        [SerializeField] float fadeOutTimeSec = 1f;

        volatile float fadeInSpeed;

        volatile float fadeOutSpeed;

        volatile float fadeSpeed;

        volatile bool firstAudioFilterIteration;

        volatile bool isFading;

        volatile bool needToDisable;

        volatile bool needToKill;

        volatile float prevAudioTime;

        public AudioSource Source => source;

        protected abstract float FilterVolume { get; set; }

        protected virtual void Awake() {
            fadeInSpeed = 1f / fadeInTimeSec;
            fadeOutSpeed = -1f / fadeOutTimeSec;
            ResetFilter();
        }

        void Update() {
            if (needToKill) {
                StopAndDestroy();
            } else if (needToDisable) {
                ResetFilter();
            }
        }

        void OnAudioFilterRead(float[] data, int channels) {
            float num = (float)AudioSettings.dspTime;
            float num2 = fadeSpeed;
            float filterVolume = FilterVolume;
            float num3 = filterVolume;

            if (firstAudioFilterIteration) {
                firstAudioFilterIteration = false;
                prevAudioTime = num;
            }

            float num4 = num - prevAudioTime;

            if (!Mathf.Approximately(num4, 0f)) {
                prevAudioTime = num;
            }

            num3 += num2 * num4;
            float num5 = Mathf.Clamp01(num3);
            int num6 = data.Length;

            for (int i = 0; i < num6; i++) {
                data[i] *= Mathf.Lerp(filterVolume, num5, i / (float)num6 - 1f);
            }

            if (!isFading) {
                FilterVolume = num5;
                return;
            }

            if (num3 <= 0f && num2 < 0f) {
                needToKill = true;
            }

            if (num3 >= 1f && num2 > 0f) {
                needToDisable = true;
            }

            FilterVolume = num5;
        }

        protected void ResetFilter() {
            enabled = false;
            needToKill = false;
            needToDisable = false;
            isFading = false;
            firstAudioFilterIteration = false;
        }

        void StartFilter(float speed) {
            fadeSpeed = speed;
            firstAudioFilterIteration = true;
            fadeSpeed = speed;
            needToKill = false;
            needToDisable = false;
            isFading = true;
            enabled = true;
        }

        public void Play(float delay = -1f) {
            if (fadeInTimeSec > 0.01f) {
                StartFilter(fadeInSpeed);
            } else {
                ResetFilter();
                FilterVolume = 1f;
            }

            PlaySound(delay);
        }

        public void Stop() {
            if (!CheckSoundIsPlaying()) {
                StopAndDestroy();
            } else if (fadeOutTimeSec > 0.01f) {
                StartFilter(fadeOutSpeed);
            } else {
                StopAndDestroy();
            }
        }

        void PlaySound(float delay) {
            if (!source.isPlaying) {
                if (delay <= 0f) {
                    source.Play();
                } else {
                    source.PlayScheduled(AudioSettings.dspTime + delay);
                }
            }
        }

        protected abstract void StopAndDestroy();

        protected abstract bool CheckSoundIsPlaying();
    }
}