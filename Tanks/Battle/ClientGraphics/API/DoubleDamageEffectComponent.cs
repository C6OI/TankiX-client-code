using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    [RequireComponent(typeof(Animator))]
    public class DoubleDamageEffectComponent : MonoBehaviour, Component {
        public Animator animator;

        public LightContainer light;

        public Renderer renderer;

        public DoubleDamageSoundEffectComponent soundEffect;

        public Color emissionColor;

        public int burningTimeInMs = 500;

        SupplyAnimationPlayer animationPlayer;

        Material ddDetailsMaterial;

        Material mainMaterial;

        SmoothHeater smoothHeater;

        Color usualEmissionColor;

        public bool Prepared { get; private set; }

        void Awake() => enabled = false;

        void Update() => smoothHeater.Update();

        public void InitEffect(SupplyEffectSettingsComponent settings) {
            animationPlayer = new SupplyAnimationPlayer(animator, AnimationParameters.DAMAGE_ACTIVE);
            mainMaterial = TankMaterialsUtil.GetMainMaterial(renderer);
            ddDetailsMaterial = TankMaterialsUtil.GetMaterialForDoubleDamageDetails(renderer);
            soundEffect.Init(transform);
            usualEmissionColor = mainMaterial.GetColor("_EmissionColor");

            smoothHeater = !settings.LightIsEnabled ? new SmoothHeater(burningTimeInMs, ddDetailsMaterial, this)
                               : new SmoothHeaterLighting(burningTimeInMs, ddDetailsMaterial, this, light);

            Prepared = true;
        }

        public void Play() => animationPlayer.StartAnimation();

        public void Stop() => animationPlayer.StopAnimation();

        void OnDamageStart() {
            soundEffect.BeginEffect();
            mainMaterial.SetColor("_EmissionColor", emissionColor);
        }

        void OnDamageStarted() => smoothHeater.Heat();

        void OnDamageStop() {
            smoothHeater.Cool();
            soundEffect.StopEffect();
            mainMaterial.SetColor("_EmissionColor", usualEmissionColor);
        }
    }
}