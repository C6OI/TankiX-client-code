using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    [RequireComponent(typeof(Animator))]
    public class NitroEffectComponent : MonoBehaviour, Component {
        public Animator animator;

        public Renderer renderer;

        public LightContainer lightContainer;

        public GameObject effectPrefab;

        public Transform effectPoints;

        public SpeedSoundEffectComponent soundEffect;

        public int burningTimeInMs = 500;

        SupplyAnimationPlayer animationPlayer;

        ParticleSystem[] effectInstances;

        int effectInstancesCount;

        SmoothHeater smoothHeater;

        public bool Prepared { get; private set; }

        void Awake() => enabled = false;

        void Update() => smoothHeater.Update();

        public void InitEffect(SupplyEffectSettingsComponent settings) {
            animationPlayer = new SupplyAnimationPlayer(animator, AnimationParameters.SPEED_ACTIVE);
            effectInstances = SupplyEffectUtil.InstantiateEffect(effectPrefab, effectPoints);
            soundEffect.Init(transform);
            Material materialForNitroDetails = TankMaterialsUtil.GetMaterialForNitroDetails(renderer);

            smoothHeater = !settings.LightIsEnabled ? new SmoothHeater(burningTimeInMs, materialForNitroDetails, this)
                               : new SmoothHeaterLighting(burningTimeInMs, materialForNitroDetails, this, lightContainer);

            effectInstancesCount = effectInstances.Length;
            Prepared = true;
        }

        public void Play() => animationPlayer.StartAnimation();

        public void Stop() => animationPlayer.StopAnimation();

        void OnSpeedStart() => soundEffect.BeginEffect();

        void OnSpeedStarted() {
            smoothHeater.Heat();

            for (int i = 0; i < effectInstancesCount; i++) {
                effectInstances[i].Play(true);
            }
        }

        void OnSpeedStop() {
            smoothHeater.Cool();

            for (int i = 0; i < effectInstancesCount; i++) {
                effectInstances[i].Stop(true);
            }

            soundEffect.StopEffect();
        }
    }
}