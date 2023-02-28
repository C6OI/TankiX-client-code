using System;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EmergencyProtectionTankShaderEffectComponent : BehaviourComponent {
        const string ROTATION_ANGLE_KEY = "_RepairRotationAngle";

        const string EMERGENCY_PROTECTION_COLOR = "_EmergencyProtectionColor";

        const string EMERGENCY_PROTECTION_FRONT_COEFF = "_EmergencyProtectionFrontCoeff";

        const string EMERGENCY_PROTECTION_NOISE_TEX = "_EmergencyProtectionNoise";

        [SerializeField] Color emergencyProtectionColor;

        [SerializeField] float duration = 1f;

        [SerializeField] float waveAnimationTime = 1f;

        [SerializeField] AnimationCurve straightStepCurve;

        [SerializeField] AnimationCurve reverseStepCurve;

        [SerializeField] Vector2 noiseTextureTiling = new(5f, 5f);

        [SerializeField] Texture2D noiseTexture;

        [SerializeField] ParticleSystem waveEffect;

        [SerializeField] bool useWaveEffect;

        [SerializeField] float delayWithWaveEffect = 0.25f;

        bool frontDirection;

        float phaseTimer;

        AnimationStates state;

        HealingGraphicEffectInputs tankEffectInput;

        int waveCount;

        int waveIterator;

        int waveTimeoutIterator;

        float waveTimeoutLength;

        WeaponHealingGraphicEffectInputs weaponEffectInputs;

        public float DelayWithWaveEffect => delayWithWaveEffect;

        public float Duration => duration;

        public ParticleSystem WaveEffect => waveEffect;

        public bool UseWaveEffect => useWaveEffect;

        void Update() {
            float deltaTime = Time.deltaTime;

            if (waveIterator + waveTimeoutIterator <= 0) {
                StopEffect();
                return;
            }

            phaseTimer += Time.deltaTime;
            float num = 0f;

            if (state == AnimationStates.TIMEOUT) {
                num = phaseTimer / waveTimeoutLength;

                if (num >= 1f) {
                    phaseTimer = 0f;
                    state = AnimationStates.WAVE;
                    waveTimeoutIterator--;
                }

                return;
            }

            num = phaseTimer / waveAnimationTime;

            if (num >= 1f) {
                phaseTimer = 0f;
                state = AnimationStates.TIMEOUT;
                frontDirection = !frontDirection;
                waveIterator--;
            } else {
                AnimationCurve animationCurve = !frontDirection ? reverseStepCurve : straightStepCurve;
                UpdateFrontCoeff(animationCurve.Evaluate(num));
            }
        }

        public void InitEffect(HealingGraphicEffectInputs tankEffectInput, WeaponHealingGraphicEffectInputs weaponEffectInputs) {
            enabled = false;
            this.tankEffectInput = tankEffectInput;
            this.weaponEffectInputs = weaponEffectInputs;
            InitTankPartInputs(tankEffectInput);
            InitTankPartInputs(weaponEffectInputs);
            phaseTimer = 0f;
        }

        void InitTankPartInputs(HealingGraphicEffectInputs inputs) {
            SkinnedMeshRenderer renderer = inputs.Renderer;
            Material[] materials = renderer.materials;
            int num = materials.Length;

            for (int i = 0; i < num; i++) {
                Material material = materials[i];
                material.SetColor("_EmergencyProtectionColor", emergencyProtectionColor);
                material.SetFloat("_EmergencyProtectionFrontCoeff", 0f);
                material.SetTexture("_EmergencyProtectionNoise", noiseTexture);
                material.SetTextureScale("_EmergencyProtectionNoise", noiseTextureTiling);
            }
        }

        public void StartEffect(Shader shader) {
            StartEffect(shader, tankEffectInput);
            StartEffect(shader, weaponEffectInputs);
            waveCount = Mathf.FloorToInt(duration / waveAnimationTime);
            ResetAnimationParameters();
            ResetAnimationWaveIterators();
            waveTimeoutLength = (duration - waveAnimationTime * waveCount) / waveTimeoutIterator;
            frontDirection = true;
            enabled = true;
        }

        void ResetAnimationParameters() {
            phaseTimer = 0f;
            state = AnimationStates.TIMEOUT;
        }

        void ResetAnimationWaveIterators() {
            waveIterator = waveCount;
            waveTimeoutIterator = waveCount + 1;
        }

        void StartEffect(Shader shader, HealingGraphicEffectInputs inputs) {
            SkinnedMeshRenderer renderer = inputs.Renderer;
            Material[] materials = renderer.materials;
            int num = materials.Length;

            for (int i = 0; i < num; i++) {
                Material material = materials[i];
                material.shader = shader;
                material.SetFloat("_EmergencyProtectionFrontCoeff", 0f);
            }
        }

        void UpdateFrontCoeff(float coeff) {
            UpdateFrontCoeff(coeff, tankEffectInput);
            UpdateFrontCoeff(coeff, weaponEffectInputs);
        }

        void UpdateFrontCoeff(float coeff, HealingGraphicEffectInputs inputs) {
            SkinnedMeshRenderer renderer = inputs.Renderer;
            Material[] materials = renderer.materials;
            int num = materials.Length;

            for (int i = 0; i < num; i++) {
                Material material = materials[i];
                material.SetFloat("_EmergencyProtectionFrontCoeff", coeff);
            }
        }

        void UpdateFrontCoeff(float coeff, WeaponHealingGraphicEffectInputs inputs) {
            SkinnedMeshRenderer renderer = inputs.Renderer;
            Material[] materials = renderer.materials;
            int num = materials.Length;
            float value = -(float)Math.PI / 180f * inputs.RotationTransform.localEulerAngles.y;

            for (int i = 0; i < num; i++) {
                Material material = materials[i];
                material.SetFloat("_RepairRotationAngle", value);
                material.SetFloat("_EmergencyProtectionFrontCoeff", coeff);
            }
        }

        public void StopEffect() {
            EngineService.Engine.NewEvent<StopEmergencyProtectionTankShaderEffectEvent>().AttachAll(tankEffectInput.Entity, weaponEffectInputs.Entity).Schedule();
            enabled = false;
        }

        enum AnimationStates {
            WAVE = 0,
            TIMEOUT = 1
        }
    }
}