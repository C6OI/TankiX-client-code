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

        float maxVolume;

        volatile bool needToDisable;

        volatile bool needToKill;

        double prevAudioTime;

        public AudioSource Source => source;

        protected abstract float FilterVolume { get; set; }

        protected virtual void Awake() {
            fadeInSpeed = 1f / fadeInTimeSec;
            fadeOutSpeed = -1f / fadeOutTimeSec;
            maxVolume = source.volume;
            ResetFilter();
        }

        void Update() {
            UpdateSoundWithinMainThread();
            ApplySourceVolume();

            if (needToKill) {
                StopAndDestroy();
            } else if (needToDisable) {
                ResetFilter();
            }
        }

        protected void ResetFilter() {
            enabled = false;
            needToKill = false;
            needToDisable = false;
            isFading = false;
            firstAudioFilterIteration = false;
        }

        void UpdateSoundWithinMainThread() {
            float deltaTime = Time.deltaTime;
            float num = fadeSpeed;
            float filterVolume = FilterVolume;
            float num2 = filterVolume;
            num2 += num * deltaTime;
            float filterVolume2 = Mathf.Clamp(num2, 0f, 1f);

            if (!isFading) {
                FilterVolume = filterVolume2;
                return;
            }

            if (num2 <= 0f && num < 0f) {
                needToKill = true;
            }

            if (num2 >= 1f && num > 0f) {
                needToDisable = true;
            }

            FilterVolume = filterVolume2;
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
            float filterVolume = 0f;

            if (fadeInTimeSec > 0.01f) {
                StartFilter(fadeInSpeed);
            } else {
                filterVolume = 1f;
                ResetFilter();
            }

            if (!source.isPlaying) {
                FilterVolume = filterVolume;
                ApplySourceVolume();
            }

            PlaySound(delay);
        }

        void ApplySourceVolume() {
            source.volume = FilterVolume * maxVolume;
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