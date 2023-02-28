using System.Collections;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    [RequireComponent(typeof(Animator))]
    public class DoubleArmorEffectComponent : MonoBehaviour, Component {
        public GameObject effectPrefab;

        public Transform effectPoints;

        public float effectTime = 2f;

        public ArmorSoundEffectComponent soundEffect;

        [SerializeField] public bool changeEmission;

        public Color emissionColor;

        public Renderer renderer;

        [HideInInspector] public Color usualEmissionColor;

        SupplyAnimationPlayer animationPlayer;

        ParticleSystem[] effectInstances;

        Material mainMaterial;

        void Awake() {
            effectInstances = SupplyEffectUtil.InstantiateEffect(effectPrefab, effectPoints);
            animationPlayer = new SupplyAnimationPlayer(GetComponent<Animator>(), AnimationParameters.ARMOR_ACTIVE);
            soundEffect.Init(transform);

            if (changeEmission) {
                mainMaterial = TankMaterialsUtil.GetMainMaterial(renderer);
                usualEmissionColor = mainMaterial.GetColor("_EmissionColor");
            }
        }

        public void Play() {
            animationPlayer.StartAnimation();
        }

        public void Stop() {
            animationPlayer.StopAnimation();
        }

        void OnArmorStart() {
            StartCoroutine(PlayTransitionCoroutine());
            soundEffect.BeginEffect();

            if (changeEmission) {
                mainMaterial.SetColor("_EmissionColor", emissionColor);
            }
        }

        void OnArmorStop() {
            soundEffect.StopEffect();

            if (changeEmission) {
                mainMaterial.SetColor("_EmissionColor", usualEmissionColor);
            }
        }

        IEnumerator PlayTransitionCoroutine() {
            for (int i = 0; i < effectInstances.Length; i++) {
                effectInstances[i].Play(true);
            }

            yield return new WaitForSeconds(effectTime);

            for (int j = 0; j < effectInstances.Length; j++) {
                effectInstances[j].Stop(true);
            }
        }
    }
}