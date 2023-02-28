using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectFadeInOutLight : MonoBehaviour {
        public float StartDelay;

        public float FadeInSpeed;

        public float FadeOutDelay;

        public float FadeOutSpeed;

        public bool FadeOutAfterCollision;

        public bool UseHideStatus;

        bool allComplited;

        bool canStart;

        bool canStartFadeOut;

        float currentIntensity;

        UpdateRankEffectSettings effectSettings;

        bool fadeInComplited;

        bool fadeOutComplited;

        Light goLight;

        bool isCollisionEnter;

        bool isIn;

        bool isInitialized;

        bool isOut;

        bool isStartDelay;

        float oldIntensity;

        float startIntensity;

        void Start() {
            GetEffectSettingsComponent(transform);

            if (effectSettings != null) {
                effectSettings.CollisionEnter += prefabSettings_CollisionEnter;
            }

            goLight = GetComponent<Light>();
            startIntensity = goLight.intensity;
            isStartDelay = StartDelay > 0.001f;
            isIn = FadeInSpeed > 0.001f;
            isOut = FadeOutSpeed > 0.001f;
            InitDefaultVariables();
            isInitialized = true;
        }

        void Update() {
            if (!canStart) {
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
                } else if (UseHideStatus && !effectSettings.IsVisible || FadeOutAfterCollision && isCollisionEnter) {
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

        void InitDefaultVariables() {
            fadeInComplited = false;
            fadeOutComplited = false;
            allComplited = false;
            canStartFadeOut = false;
            isCollisionEnter = false;
            oldIntensity = 0f;
            currentIntensity = 0f;
            canStart = false;
            goLight.intensity = !isIn ? startIntensity : 0f;

            if (isStartDelay) {
                Invoke("SetupStartDelay", StartDelay);
            } else {
                canStart = true;
            }

            if (!isIn) {
                if (!FadeOutAfterCollision) {
                    Invoke("SetupFadeOutDelay", FadeOutDelay);
                }

                oldIntensity = startIntensity;
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
            currentIntensity = oldIntensity + Time.deltaTime / FadeInSpeed * startIntensity;

            if (currentIntensity >= startIntensity) {
                fadeInComplited = true;

                if (!isOut) {
                    allComplited = true;
                }

                currentIntensity = startIntensity;
                Invoke("SetupFadeOutDelay", FadeOutDelay);
            }

            goLight.intensity = currentIntensity;
            oldIntensity = currentIntensity;
        }

        void FadeOut() {
            currentIntensity = oldIntensity - Time.deltaTime / FadeOutSpeed * startIntensity;

            if (currentIntensity <= 0f) {
                currentIntensity = 0f;
                fadeOutComplited = true;
                allComplited = true;
            }

            goLight.intensity = currentIntensity;
            oldIntensity = currentIntensity;
        }
    }
}