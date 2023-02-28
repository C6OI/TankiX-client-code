using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class CameraShakeInstance {
        Vector3 amt;

        public bool deleteOnInactive = true;

        float fadeInDuration;

        float fadeOutDuration;
        public float magnitude;

        public Vector3 positionInfluence;

        public Vector3 rotationInfluence;

        public float roughness;

        bool sustain;

        float tick;

        public CameraShakeInstance() {
            ResetFields();
        }

        public float ScaleRoughness { get; set; } = 1f;

        public float ScaleMagnitude { get; set; } = 1f;

        public float NormalizedFadeTime { get; private set; }

        bool IsShaking => NormalizedFadeTime > 0f || sustain;

        bool IsFadingOut => !sustain && NormalizedFadeTime > 0f;

        bool IsFadingIn => NormalizedFadeTime < 1f && sustain && fadeInDuration > 0f;

        public CameraShakeState CurrentState {
            get {
                if (IsFadingIn) {
                    return CameraShakeState.FadingIn;
                }

                if (IsFadingOut) {
                    return CameraShakeState.FadingOut;
                }

                if (IsShaking) {
                    return CameraShakeState.Sustained;
                }

                return CameraShakeState.Inactive;
            }
        }

        void ResetFields() {
            magnitude = 0f;
            roughness = 0f;
            positionInfluence = Vector3.zero;
            rotationInfluence = Vector3.zero;
            deleteOnInactive = true;
            ScaleRoughness = 1f;
            ScaleMagnitude = 1f;
            fadeInDuration = 0f;
            fadeOutDuration = 0f;
            sustain = false;
            NormalizedFadeTime = 0f;
            tick = 0f;
            amt = Vector3.zero;
        }

        public CameraShakeInstance Init(float magnitude, float roughness, float fadeInTime, float fadeOutTime) {
            ResetFields();
            this.magnitude = magnitude;
            fadeOutDuration = fadeOutTime;
            fadeInDuration = fadeInTime;
            this.roughness = roughness;

            if (fadeInTime > 0f) {
                sustain = true;
                NormalizedFadeTime = 0f;
            } else {
                sustain = false;
                NormalizedFadeTime = 1f;
            }

            tick = Random.Range(-100, 100);
            return this;
        }

        public CameraShakeInstance Init(float magnitude, float roughness) {
            ResetFields();
            this.magnitude = magnitude;
            this.roughness = roughness;
            sustain = true;
            tick = Random.Range(-100, 100);
            return this;
        }

        public Vector3 UpdateShake() {
            amt.x = Mathf.PerlinNoise(tick, 0f) - 0.5f;
            amt.y = Mathf.PerlinNoise(0f, tick) - 0.5f;
            amt.z = Mathf.PerlinNoise(tick, tick) - 0.5f;

            if (fadeInDuration > 0f && sustain) {
                if (NormalizedFadeTime < 1f) {
                    NormalizedFadeTime += Time.deltaTime / fadeInDuration;
                } else if (fadeOutDuration > 0f) {
                    sustain = false;
                }
            }

            if (!sustain) {
                NormalizedFadeTime -= Time.deltaTime / fadeOutDuration;
            }

            if (sustain) {
                tick += Time.deltaTime * roughness * ScaleRoughness;
            } else {
                tick += Time.deltaTime * roughness * ScaleRoughness * NormalizedFadeTime;
            }

            return amt * magnitude * ScaleMagnitude * NormalizedFadeTime;
        }

        public void StartFadeOut(float fadeOutTime) {
            if (fadeOutTime == 0f) {
                NormalizedFadeTime = 0f;
            }

            fadeOutDuration = fadeOutTime;
            fadeInDuration = 0f;
            sustain = false;
        }

        public void StartFadeIn(float fadeInTime) {
            if (fadeInTime == 0f) {
                NormalizedFadeTime = 1f;
            }

            fadeInDuration = fadeInTime;
            fadeOutDuration = 0f;
            sustain = true;
        }
    }
}