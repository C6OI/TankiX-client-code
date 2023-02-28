using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HealthFeedbackGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void InitBattleCameraForHealthFeedback(NodeAddedEvent e, BattleCameraNode camera, SingleNode<HealthFeedbackMapEffectMaterialComponent> mapEffect,
            SingleNode<GameTankSettingsComponent> settings) {
            if (settings.component.HealthFeedbackEnabled) {
                GameObject gameObject = camera.camera.UnityCamera.gameObject;
                HealthFeedbackPostEffect healthFeedbackPostEffect = gameObject.AddComponent<HealthFeedbackPostEffect>();
                healthFeedbackPostEffect.Init(mapEffect.component.SourceMaterial);
                camera.Entity.AddComponent(new HealthFeedbackCameraPreparedComponent(healthFeedbackPostEffect));
            }
        }

        [OnEventFire]
        public void DisableHealthVignette(NodeAddedEvent evt, SelfTankNode tank, ReadyBattleCameraNode camera) {
            DisableHealthVignette(camera);
        }

        [OnEventFire]
        public void DisableHealthVignette(NodeRemoveEvent evt, SelfTankNode tank, [Context] [JoinAll] ReadyBattleCameraNode camera) {
            DisableHealthVignette(camera);
        }

        [OnEventFire]
        public void DisableHealthVignette(NodeAddedEvent evt, SelfDeadTankNode tank, ReadyBattleCameraNode camera) {
            camera.healthFeedbackCameraPrepared.TargetIntensity = 0f;
        }

        [OnEventFire]
        public void ChangeHealthVignette(HealthChangedEvent evt, SelfActiveTankNode tank, [JoinAll] ReadyBattleCameraNode camera) {
            float currentHealth = tank.health.CurrentHealth;
            float maxHealth = tank.health.MaxHealth;
            float value = currentHealth / maxHealth;
            camera.healthFeedbackCameraPrepared.TargetIntensity = 1f - Mathf.Clamp01(value);
        }

        [OnEventFire]
        public void ChangeHealthVignette(UpdateEvent evt, SelfTankNode tank, [JoinAll] ReadyBattleCameraNode camera,
            [JoinAll] SingleNode<HealthFeedbackMapEffectMaterialComponent> mapEffect) {
            float damageIntensity = camera.healthFeedbackCameraPrepared.HealthFeedbackPostEffect.DamageIntensity;
            float targetIntensity = camera.healthFeedbackCameraPrepared.TargetIntensity;
            float f = targetIntensity - damageIntensity;

            if (!(Mathf.Abs(f) <= 0.0001f)) {
                float num = Mathf.Sign(f) * mapEffect.component.IntensitySpeed;
                float num2 = damageIntensity;
                num2 += num * evt.DeltaTime;
                num2 = !(num > 0f) ? Mathf.Clamp(num2, targetIntensity, damageIntensity) : Mathf.Clamp(num2, damageIntensity, targetIntensity);
                camera.healthFeedbackCameraPrepared.HealthFeedbackPostEffect.DamageIntensity = num2;
            }
        }

        void DisableHealthVignette(ReadyBattleCameraNode camera) {
            camera.healthFeedbackCameraPrepared.TargetIntensity = 0f;
            camera.healthFeedbackCameraPrepared.HealthFeedbackPostEffect.DamageIntensity = 0f;
        }

        public class BattleCameraNode : Node {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class ReadyBattleCameraNode : BattleCameraNode {
            public HealthFeedbackCameraPreparedComponent healthFeedbackCameraPrepared;
        }

        public class SelfTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public HealthComponent health;
            public SelfTankComponent selfTank;
        }

        public class SelfDeadTankNode : SelfTankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfActiveTankNode : SelfTankNode {
            public TankActiveStateComponent tankActiveState;
        }
    }
}