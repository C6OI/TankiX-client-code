using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour {
        [SerializeField] float playingDelaySec;

        [SerializeField] float playingOffsetSec;

        [SerializeField] float fadeOutTimeSec = 0.5f;

        [SerializeField] float fadeInTimeSec;

        [Range(0f, 1f)] [SerializeField] float minVolume;

        [Range(0f, 1f)] [SerializeField] float maxVolume = 1f;

        [SerializeField] AudioSource source;

        float currentFadeSpeed;

        float currentVolume;

        float fadeInSpeed;

        float fadeOutSpeed;

        float playingDelayTimer;

        SoundControllerStates state;

        SoundControllerStates State {
            get => state;
            set {
                playingDelayTimer = 0f;
                SoundControllerStates soundControllerStates = state;
                bool flag = soundControllerStates != value;
                state = value;

                switch (value) {
                    case SoundControllerStates.FADE_IN:
                        if (flag) {
                            if (soundControllerStates != SoundControllerStates.ACTIVE) {
                                StartFadingPhase();
                            } else {
                                State = SoundControllerStates.ACTIVE;
                            }
                        }

                        break;

                    case SoundControllerStates.FADE_OUT:
                        if (flag) {
                            if (soundControllerStates != SoundControllerStates.INACTIVE) {
                                StartFadingPhase();
                            } else {
                                State = SoundControllerStates.INACTIVE;
                            }
                        }

                        break;

                    case SoundControllerStates.INACTIVE:
                        SetInactiveParams();
                        break;

                    case SoundControllerStates.ACTIVE:
                        SetActiveParams();
                        break;

                    default:
                        throw new ArgumentException("Invalid sound Controller state");
                }
            }
        }

        public AudioSource Source => source;

        float CurrentVolume {
            get => currentVolume;
            set {
                currentVolume = value;
                currentVolume = Mathf.Clamp(value, minVolume, maxVolume);
                source.volume = currentVolume;
            }
        }

        public float FadeInTimeSec {
            get => fadeInTimeSec;
            set {
                fadeInTimeSec = value;
                fadeInSpeed = CalculateFadingSpeed(fadeInTimeSec);
            }
        }

        public float FadeOutTimeSec {
            get => fadeOutTimeSec;
            set {
                fadeOutTimeSec = value;
                fadeOutSpeed = 0f - CalculateFadingSpeed(fadeOutTimeSec);
            }
        }

        public float PlayingDelaySec {
            get => playingDelaySec;
            set => playingDelaySec = value;
        }

        public float PlayingOffsetSec {
            get => playingOffsetSec;
            set => playingOffsetSec = value;
        }

        public float MinVolume {
            get => minVolume;
            set {
                minVolume = value;
                source.volume = Mathf.Clamp(currentVolume, minVolume, maxVolume);
            }
        }

        public float MaxVolume {
            get => maxVolume;
            set {
                maxVolume = value;
                source.volume = Mathf.Clamp(currentVolume, minVolume, maxVolume);
            }
        }

        void Awake() {
            source.time = playingOffsetSec;
            fadeInSpeed = CalculateFadingSpeed(fadeInTimeSec);
            fadeOutSpeed = 0f - CalculateFadingSpeed(fadeOutTimeSec);
            State = SoundControllerStates.INACTIVE;
        }

        void Update() {
            float deltaTime = Time.deltaTime;

            if (playingDelayTimer > 0f) {
                playingDelayTimer -= deltaTime;

                if (playingDelayTimer > 0f) {
                    return;
                }
            }

            if (State == SoundControllerStates.INACTIVE) {
                enabled = false;
                return;
            }

            if (State == SoundControllerStates.ACTIVE) {
                enabled = false;
                return;
            }

            CurrentVolume += currentFadeSpeed * deltaTime;

            if (CurrentVolume <= minVolume) {
                State = SoundControllerStates.INACTIVE;
            } else if (CurrentVolume >= maxVolume) {
                State = SoundControllerStates.ACTIVE;
            }
        }

        float CalculateFadingSpeed(float fadingTime) => !(fadingTime > 0f) ? 0f : 1f / fadingTime;

        void SetInactiveParams() {
            CurrentVolume = minVolume;
            enabled = false;
            StopSound();
        }

        void SetActiveParams() {
            CurrentVolume = maxVolume;
            enabled = false;
            StartSound();
        }

        void StartFadingPhase() {
            float num = 0f;
            float num2 = 0f;
            bool flag = false;
            SoundControllerStates soundControllerStates;

            switch (state) {
                case SoundControllerStates.FADE_IN:
                    num = fadeInTimeSec;
                    num2 = fadeInSpeed;
                    soundControllerStates = SoundControllerStates.ACTIVE;
                    flag = CurrentVolume >= maxVolume;
                    StartSound();
                    break;

                case SoundControllerStates.FADE_OUT:
                    num = fadeOutTimeSec;
                    num2 = fadeOutSpeed;
                    soundControllerStates = SoundControllerStates.INACTIVE;
                    flag = CurrentVolume <= minVolume;
                    break;

                default:
                    throw new ArgumentException("Fading phase doesn't exist");
            }

            if (flag || num == 0f) {
                State = soundControllerStates;
                return;
            }

            currentFadeSpeed = num2;
            enabled = true;
        }

        void StartSound() {
            if (!source.isPlaying) {
                source.time = playingOffsetSec;
                source.PlayScheduled(AudioSettings.dspTime + PlayingDelaySec);
                playingDelayTimer = PlayingDelaySec;
                enabled = true;
            }
        }

        void StopSound() => source.Stop();

        public void FadeIn() => State = SoundControllerStates.FADE_IN;

        public void SetSoundActive() => State = SoundControllerStates.ACTIVE;

        public void FadeOut() => State = SoundControllerStates.FADE_OUT;

        public void StopImmediately() => State = SoundControllerStates.INACTIVE;

        enum SoundControllerStates {
            INITIAL = 0,
            ACTIVE = 1,
            INACTIVE = 2,
            FADE_IN = 3,
            FADE_OUT = 4
        }
    }
}