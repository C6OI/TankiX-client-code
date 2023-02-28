using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class DissolvingEffectUnityComponent : BehaviourComponent {
        const float MAX_DISSOLVE_VALUE = 1.01f;

        const string DISSOLVE_MAP_STRING_KEY = "_DissolveMap";

        const string DISSOLVE_COEFF_STRING_KEY = "_DissolveCoeff";

        const string DISTORTION_COEFF_STRING_KEY = "_DistortionCoeff";

        [SerializeField] Texture2D[] dissolveMaps;

        [SerializeField] Shader invisibilityEffectTransitionShader;

        [SerializeField] float phaseTime = 1f;

        [SerializeField] float offsetPhaseTime = 0.3f;

        [SerializeField] float maxDistortion = 70f;

        [SerializeField] Vector2 dissolveMapScale = new(2f, 2f);

        [SerializeField] List<Renderer> renderers;

        [SerializeField] AnimationCurve dissolvingCurve;

        [SerializeField] SoundController soundInstance;

        int dissolveCoeffIntKey;

        int dissolveMapIntKey;

        int distortionCoeffIntKey;

        Entity entity;

        bool inited;

        List<Material> materials;

        int materialsLength;

        List<Shader> savedShaders;

        float timer;

        public Shader InvisibilityEffectTransitionShader => invisibilityEffectTransitionShader;

        public void Start() {
            StartEffect();
        }

        void Update() {
            if (inited) {
                float num = 0f;
                float num2 = 0f;
                float num3 = 0f;
                timer -= Time.deltaTime;
                num = timer / phaseTime;
                num2 = num - offsetPhaseTime;
                num3 = num;
                ApplyTransition(dissolvingCurve, num3, num2);

                if (num3 <= 0f) {
                    ReturnMaterials();
                    enabled = false;
                }
            }
        }

        public void StartEffect() {
            timer = phaseTime;
            materials = new List<Material>();

            foreach (Renderer renderer in renderers) {
                materials.AddRange(renderer.materials);
            }

            materialsLength = materials.Count;
            dissolveCoeffIntKey = Shader.PropertyToID("_DissolveCoeff");
            dissolveMapIntKey = Shader.PropertyToID("_DissolveMap");
            distortionCoeffIntKey = Shader.PropertyToID("_DistortionCoeff");
            ConfigureMaterials(1.01f, maxDistortion, invisibilityEffectTransitionShader);
            enabled = true;

            if ((bool)soundInstance) {
                soundInstance.SetSoundActive();
            }

            inited = true;
        }

        void ConfigureMaterials(float dissolveVal, float distortionVal, Shader shader) {
            Texture2D value = dissolveMaps[Random.Range(0, dissolveMaps.Length)];
            savedShaders = new List<Shader>(dissolveMaps.Length);

            for (int i = 0; i < materialsLength; i++) {
                Material material = materials[i];
                savedShaders.Add(material.shader);
                material.shader = shader;
                material.SetTexture(dissolveMapIntKey, value);
                material.SetTextureScale("_DissolveMap", dissolveMapScale);
                material.SetFloat(dissolveCoeffIntKey, dissolveVal);
                material.SetFloat(distortionCoeffIntKey, distortionVal);
            }
        }

        void ReturnMaterials() {
            int num = 0;

            foreach (Material material in materials) {
                material.shader = savedShaders[num++];
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
    }
}