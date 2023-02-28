using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class TankInvisibilityEffectUnityComponent : BehaviourComponent {
        const float MAX_DISSOLVE_VALUE = 1.01f;

        const string DISSOLVE_MAP_STRING_KEY = "_DissolveMap";

        const string DISSOLVE_COEFF_STRING_KEY = "_DissolveCoeff";

        const string DISTORTION_COEFF_STRING_KEY = "_DistortionCoeff";

        [SerializeField] Texture2D[] dissolveMaps;

        [SerializeField] Shader invisibilityEffectTransitionShader;

        [SerializeField] Shader invisibilityEffectShader;

        [SerializeField] float phaseTime = 1f;

        [SerializeField] float offsetPhaseTime = 0.3f;

        [SerializeField] float maxDistortion = 70f;

        [SerializeField] Vector2 dissolveMapScale = new(2f, 2f);

        [SerializeField] AnimationCurve dissolvingFrontCurve;

        [SerializeField] AnimationCurve dissolvingBackCurve;

        [SerializeField] SoundController activationSoundInstance;

        [SerializeField] SoundController deactivationSoundInstance;

        int dissolveCoeffIntKey;

        int dissolveMapIntKey;

        int distortionCoeffIntKey;

        EffectStates effectState;

        Entity entity;

        List<Material> materials;

        int materialsLength;

        float timer;

        EffectStates EffectState {
            get => effectState;
            set {
                EffectStates effectStates = effectState;
                effectState = value;

                switch (effectState) {
                    case EffectStates.IDLE:
                        SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectIdleState>();
                        timer = 0f;
                        enabled = false;
                        activationSoundInstance.FadeOut();
                        break;

                    case EffectStates.WORKING:
                        SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectWorkingState>();
                        timer = phaseTime;
                        SetRandomDissolveTextures(1.01f, maxDistortion, invisibilityEffectShader);
                        enabled = false;
                        deactivationSoundInstance.FadeOut();
                        break;

                    case EffectStates.ACTIVATION:
                        SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectActivationState>();

                        if (effectStates == EffectStates.IDLE) {
                            SetRandomDissolveTextures(0f, 0f, invisibilityEffectTransitionShader);
                        }

                        enabled = true;
                        activationSoundInstance.SetSoundActive();
                        deactivationSoundInstance.FadeOut();
                        break;

                    case EffectStates.DEACTIVATION:
                        SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectDeactivationState>();

                        if (effectStates == EffectStates.WORKING) {
                            SetRandomDissolveTextures(1.01f, maxDistortion, invisibilityEffectTransitionShader);
                        }

                        enabled = true;
                        activationSoundInstance.FadeOut();
                        deactivationSoundInstance.SetSoundActive();
                        break;
                }
            }
        }

        public Shader InvisibilityEffectTransitionShader => invisibilityEffectTransitionShader;

        public Shader InvisibilityEffectShader => invisibilityEffectShader;

        void Update() {
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;

            switch (effectState) {
                case EffectStates.ACTIVATION:
                    timer += Time.deltaTime;
                    num = timer / phaseTime;
                    num2 = num - offsetPhaseTime;
                    num3 = num;
                    ApplyTransition(dissolvingFrontCurve, num3, num2);

                    if (num2 >= 1f) {
                        EffectState = EffectStates.WORKING;
                    }

                    break;

                case EffectStates.DEACTIVATION:
                    timer -= Time.deltaTime;
                    num = timer / phaseTime;
                    num2 = num - offsetPhaseTime;
                    num3 = num;
                    ApplyTransition(dissolvingBackCurve, num3, num2);

                    if (num3 <= 0f) {
                        EffectState = EffectStates.IDLE;
                    }

                    break;
            }
        }

        public void ConfigureEffect(Entity entity, bool fullInvisibly, params Renderer[] renderers) {
            this.entity = entity;
            timer = 0f;
            materials = new List<Material>();

            foreach (Renderer renderer in renderers) {
                materials.AddRange(renderer.materials);
            }

            materialsLength = materials.Count;
            effectState = EffectStates.IDLE;
            dissolveCoeffIntKey = Shader.PropertyToID("_DissolveCoeff");
            dissolveMapIntKey = Shader.PropertyToID("_DissolveMap");
            distortionCoeffIntKey = Shader.PropertyToID("_DistortionCoeff");

            if (fullInvisibly) {
                maxDistortion = 0f;
            }
        }

        public void ResetEffect() {
            EffectState = EffectStates.IDLE;
        }

        public void ActivateEffect() {
            EffectState = EffectStates.ACTIVATION;
        }

        public void DeactivateEffect() {
            EffectState = EffectStates.DEACTIVATION;
        }

        void SetRandomDissolveTextures(float dissolveVal, float distortionVal, Shader shader) {
            Texture2D value = dissolveMaps[Random.Range(0, dissolveMaps.Length)];

            for (int i = 0; i < materialsLength; i++) {
                Material material = materials[i];
                material.shader = shader;
                material.SetTexture(dissolveMapIntKey, value);
                material.SetTextureScale("_DissolveMap", dissolveMapScale);
                material.SetFloat(dissolveCoeffIntKey, dissolveVal);
                material.SetFloat(distortionCoeffIntKey, distortionVal);
            }
        }

        void ApplyTransition(AnimationCurve dissolveCurve, float dissolveCoeff, float cloackCoeff) {
            float value = Mathf.Lerp(0f, 1.01f, dissolveCurve.Evaluate(dissolveCoeff));
            float value2 = Mathf.Lerp(0f, maxDistortion, cloackCoeff);

            for (int i = 0; i < materialsLength; i++) {
                Material material = materials[i];
                material.SetFloat(dissolveCoeffIntKey, value);
                material.SetFloat(distortionCoeffIntKey, value2);
            }
        }

        void SwitchEntityState<T>() where T : Node {
            EngineService.Engine.ScheduleEvent(new TankInvisibilityEffectSwitchStateEvent<T>(), entity);
        }

        enum EffectStates {
            IDLE = 0,
            WORKING = 1,
            ACTIVATION = 2,
            DEACTIVATION = 3
        }
    }
}