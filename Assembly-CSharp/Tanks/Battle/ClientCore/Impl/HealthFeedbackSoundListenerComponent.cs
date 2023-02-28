using System;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class HealthFeedbackSoundListenerComponent : BehaviourComponent {
        [SerializeField] AudioReverbFilter filter;

        [SerializeField] HealthFeedbackListenerPreset normalHealthPreset;

        [SerializeField] HealthFeedbackListenerPreset lowHealthPreset;

        [SerializeField] float maxHealthPercentForSound = 0.3f;

        [SerializeField] float enterTimeSec = 0.5f;

        [SerializeField] float exitTimeSec = 0.5f;

        [SerializeField] AnimationCurve toLowHealthStateCurve;

        [SerializeField] AnimationCurve toNormalHealthStateCurve;

        AnimationCurve curve;

        float presetInterpolator;

        float speed;

        public float MaxHealthPercentForSound => maxHealthPercentForSound;

        void Update() {
            bool flag = presetInterpolator >= 1f;
            presetInterpolator += speed * Time.deltaTime;
            bool flag2 = presetInterpolator >= 1f;

            if (presetInterpolator <= 0f) {
                enabled = false;
            } else if (!flag || !flag2) {
                ApplyPresetInterpolator();
            }
        }

        void OnEnable() {
            filter.enabled = true;
        }

        void OnDisable() {
            presetInterpolator = 0f;

            if ((bool)filter) {
                filter.enabled = false;
            }
        }

        void ApplyPresetInterpolator() {
            presetInterpolator = Mathf.Clamp01(presetInterpolator);
            float t = curve.Evaluate(presetInterpolator);
            filter.dryLevel = Mathf.Lerp(normalHealthPreset.DryLevel, lowHealthPreset.DryLevel, t);
            filter.room = Mathf.Lerp(normalHealthPreset.Room, lowHealthPreset.Room, t);
            filter.roomHF = Mathf.Lerp(normalHealthPreset.RoomHf, lowHealthPreset.RoomHf, t);
            filter.roomLF = Mathf.Lerp(normalHealthPreset.RoomLf, lowHealthPreset.RoomLf, t);
            filter.decayTime = Mathf.Lerp(normalHealthPreset.DecayTime, lowHealthPreset.DecayTime, t);
            filter.decayHFRatio = Mathf.Lerp(normalHealthPreset.DecayHfRatio, lowHealthPreset.DecayHfRatio, t);
            filter.reflectionsLevel = Mathf.Lerp(normalHealthPreset.ReflectionsLevel, lowHealthPreset.ReflectionsLevel, t);
            filter.reflectionsDelay = Mathf.Lerp(normalHealthPreset.ReflectionsDelay, lowHealthPreset.ReflectionsDelay, t);
            filter.reverbLevel = Mathf.Lerp(normalHealthPreset.ReverbLevel, lowHealthPreset.ReverbLevel, t);
            filter.reverbDelay = Mathf.Lerp(normalHealthPreset.ReverbDelay, lowHealthPreset.ReverbDelay, t);
            filter.hfReference = Mathf.Lerp(normalHealthPreset.HfReference, lowHealthPreset.HfReference, t);
            filter.lfReference = Mathf.Lerp(normalHealthPreset.LfReference, lowHealthPreset.LfReference, t);
            filter.diffusion = Mathf.Lerp(normalHealthPreset.Diffusion, lowHealthPreset.Diffusion, t);
            filter.density = Mathf.Lerp(normalHealthPreset.Density, lowHealthPreset.Density, t);
        }

        public void SwitchToLowHealthMode() {
            StartRunning(1f / enterTimeSec, toLowHealthStateCurve);
        }

        public void SwitchToNormalHealthMode() {
            StartRunning(-1f / exitTimeSec, toNormalHealthStateCurve);
        }

        public void ResetHealthFeedbackData() {
            enabled = false;
            speed = 0f;
            presetInterpolator = 0f;
        }

        void StartRunning(float speed, AnimationCurve curve) {
            this.speed = speed;
            this.curve = curve;
            ApplyPresetInterpolator();
            enabled = true;
        }

        [Serializable]
        class HealthFeedbackListenerPreset {
            [SerializeField] float dryLevel;

            [SerializeField] float room;

            [SerializeField] float roomHF;

            [SerializeField] float roomLF;

            [SerializeField] float decayTime;

            [SerializeField] float decayHFRatio;

            [SerializeField] float reflectionsLevel;

            [SerializeField] float reflectionsDelay;

            [SerializeField] float reverbLevel;

            [SerializeField] float reverbDelay;

            [SerializeField] float hfReference;

            [SerializeField] float lfReference;

            [SerializeField] float diffusion;

            [SerializeField] float density;

            public float DryLevel => dryLevel;

            public float Room => room;

            public float RoomHf => roomHF;

            public float DecayTime => decayTime;

            public float DecayHfRatio => decayHFRatio;

            public float RoomLf => roomLF;

            public float ReflectionsLevel => reflectionsLevel;

            public float ReflectionsDelay => reflectionsDelay;

            public float ReverbLevel => reverbLevel;

            public float ReverbDelay => reverbDelay;

            public float HfReference => hfReference;

            public float LfReference => lfReference;

            public float Diffusion => diffusion;

            public float Density => density;
        }
    }
}