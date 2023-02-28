using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectFadeInOutSound : MonoBehaviour {
        public float MaxVolume = 1f;

        public float StartDelay;

        public float FadeInSpeed;

        public float FadeOutDelay;

        public float FadeOutSpeed;

        public bool FadeOutAfterCollision;

        public bool UseHideStatus;

        bool allComplited;

        AudioSource audioSource;

        bool canStart;

        bool canStartFadeOut;

        float currentVolume;

        UpdateRankEffectSettings effectSettings;

        bool fadeInComplited;

        bool fadeOutComplited;

        bool isCollisionEnter;

        bool isIn;

        bool isInitialized;

        bool isOut;

        bool isStartDelay;

        float oldVolume;

        void Start() {
            GetEffectSettingsComponent(transform);

            if (effectSettings != null) {
                effectSettings.CollisionEnter += prefabSettings_CollisionEnter;
            }

            InitSource();
        }

        void Update() {
            if (!canStart || audioSource == null) {
                return;
            }

            if (effectSettings != null && UseHideStatus && allComplited && effectSettings.IsVisible) {
                allComplited = false;
                fadeInComplited = false;
                fadeOutComplited = false;
                InitDefaultVariables();
            }

            if (isIn && !fadeInComplited) {
                if (effectSettings == null) {
                    FadeIn();
                } else if (UseHideStatus && effectSettings.IsVisible || !UseHideStatus) {
                    FadeIn();
                }
            }

            if (isOut && !fadeOutComplited && canStartFadeOut) {
                if (effectSettings == null || !UseHideStatus && !FadeOutAfterCollision) {
                    FadeOut();
                } else if (UseHideStatus && !effectSettings.IsVisible || isCollisionEnter) {
                    FadeOut();
                }
            }
        }

        void OnEnable() {
            if (isInitialized) {
                InitDefaultVariables();
            }
        }

        void GetEffectSettingsComponent(Transform tr) {
            Transform parent = tr.parent;

            if (parent != null) {
                effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();

                if (effectSettings == null) {
                    GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        void InitSource() {
            if (!isInitialized) {
                audioSource = GetComponent<AudioSource>();

                if (!(audioSource == null)) {
                    isStartDelay = StartDelay > 0.001f;
                    isIn = FadeInSpeed > 0.001f;
                    isOut = FadeOutSpeed > 0.001f;
                    InitDefaultVariables();
                    isInitialized = true;
                }
            }
        }

        void InitDefaultVariables() {
            fadeInComplited = false;
            fadeOutComplited = false;
            allComplited = false;
            canStartFadeOut = false;
            isCollisionEnter = false;
            oldVolume = 0f;
            currentVolume = MaxVolume;

            if (isIn) {
                currentVolume = 0f;
            }

            audioSource.volume = currentVolume;

            if (isStartDelay) {
                Invoke("SetupStartDelay", StartDelay);
            } else {
                canStart = true;
            }

            if (!isIn) {
                if (!FadeOutAfterCollision) {
                    Invoke("SetupFadeOutDelay", FadeOutDelay);
                }

                oldVolume = MaxVolume;
            }
        }

        void prefabSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e) {
            isCollisionEnter = true;

            if (!isIn && FadeOutAfterCollision) {
                Invoke("SetupFadeOutDelay", FadeOutDelay);
            }
        }

        void SetupStartDelay() {
            canStart = true;
        }

        void SetupFadeOutDelay() {
            canStartFadeOut = true;
        }

        void FadeIn() {
            currentVolume = oldVolume + Time.deltaTime / FadeInSpeed * MaxVolume;

            if (currentVolume >= MaxVolume) {
                fadeInComplited = true;

                if (!isOut) {
                    allComplited = true;
                }

                currentVolume = MaxVolume;
                Invoke("SetupFadeOutDelay", FadeOutDelay);
            }

            audioSource.volume = currentVolume;
            oldVolume = currentVolume;
        }

        void FadeOut() {
            currentVolume = oldVolume - Time.deltaTime / FadeOutSpeed * MaxVolume;

            if (currentVolume <= 0f) {
                currentVolume = 0f;
                fadeOutComplited = true;
                allComplited = true;
            }

            audioSource.volume = currentVolume;
            oldVolume = currentVolume;
        }
    }
}