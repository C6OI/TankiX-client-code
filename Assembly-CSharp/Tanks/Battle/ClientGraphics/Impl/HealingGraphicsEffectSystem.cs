using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HealingGraphicsEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitRepairGraphicEffect(NodeAddedEvent evt, TankActiveStateNode tank, [JoinByTank] [Context] WeaponNode weapon) {
            InitRepairGraphicEffect(tank, weapon);
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent evt, ReadyTankActiveStateNode tank) {
            tank.healingGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent evt, HealingEffectNode effect, [JoinByTank] ReadyTankNode tank) {
            tank.healingGraphicEffect.StopEffect();
        }

        void InitRepairGraphicEffect(TankNode tank, WeaponNode weapon) {
            tank.Entity.RemoveComponentIfPresent<HealingGraphicsEffectReadyComponent>();

            tank.healingGraphicEffect.InitRepairGraphicsEffect(new HealingGraphicEffectInputs(tank.Entity, tank.baseRenderer.Renderer as SkinnedMeshRenderer),
                new WeaponHealingGraphicEffectInputs(weapon.Entity, weapon.weaponVisualRoot.transform, weapon.baseRenderer.Renderer as SkinnedMeshRenderer),
                tank.visualMountPoint.MountPoint, tank.tankSoundRoot.SoundRootTransform);

            tank.Entity.AddComponent<HealingGraphicsEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartRepairEffect(NodeAddedEvent evt, HealingEffectNode effect, [JoinByTank] [Context] ReadyTankNode tank, [JoinByTank] [Context] WeaponNode weapon) {
            NewEvent(new StartHealingGraphicsEffectEvent(effect.durationConfig.Duration / 1000f)).AttachAll(tank, weapon).Schedule();
        }

        [OnEventFire]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankNode tank) {
            ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.REPAIR_EFFECT), tank);
        }

        [OnEventFire]
        public void StopEmergencyProtectionEffect(StartHealingGraphicsEffectEvent evt, EmergencyProtectionReadyTankNode tank) {
            tank.simpleEmergencyProtectionGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopLifestealEffect(StartHealingGraphicsEffectEvent evt, LifestealTankReadyNode tank) {
            tank.lifestealGraphicsEffect.StopEffect();
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectIdleStateNode tank) {
            tank.healingGraphicEffect.StartEffect(tank.tankShader.TransparentShader, evt.Duration);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectWorkingStateNode tank) {
            tank.healingGraphicEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectShader, evt.Duration);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectActivationStateNode tank) {
            tank.healingGraphicEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader, evt.Duration);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectDeactivationStateNode tank) {
            tank.healingGraphicEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader, evt.Duration);
        }

        [OnEventFire]
        public void StopRepairEffect(StopHealingGraphicsEffectEvent evt, ReadyTankNode tank) {
            ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.REPAIR_EFFECT, false), tank);
        }

        public class TankNode : Node {
            public BaseRendererComponent baseRenderer;

            public HealingGraphicEffectComponent healingGraphicEffect;

            public RendererInitializedComponent rendererInitialized;

            public TankGroupComponent tankGroup;
            public TankInvisibilityEffectUnityComponent tankInvisibilityEffectUnity;

            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;

            public TankShaderComponent tankShader;

            public TankSoundRootComponent tankSoundRoot;

            public VisualMountPointComponent visualMountPoint;
        }

        public class ReadyTankNode : TankNode {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public HealingGraphicsEffectReadyComponent healingGraphicsEffectReady;
        }

        public class ReadyTankInvisibilityEffectIdleStateNode : ReadyTankNode {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class ReadyTankInvisibilityEffectWorkingStateNode : ReadyTankNode {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class ReadyTankInvisibilityEffectActivationStateNode : ReadyTankNode {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class ReadyTankInvisibilityEffectDeactivationStateNode : ReadyTankNode {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class TankActiveStateNode : TankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class ReadyTankActiveStateNode : TankNode {
            public HealingGraphicsEffectReadyComponent healingGraphicsEffectReady;
            public TankActiveStateComponent tankActiveState;
        }

        public class HealingEffectNode : Node {
            public DurationConfigComponent durationConfig;
            public HealingEffectComponent healingEffect;

            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererInitializedComponent rendererInitialized;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class EmergencyProtectionReadyTankNode : ReadyTankNode {
            public EmergencyProtectionTankShaderEffectReadyComponent emergencyProtectionTankShaderEffectReady;
            public SimpleEmergencyProtectionGraphicEffectComponent simpleEmergencyProtectionGraphicEffect;
        }

        public class LifestealTankReadyNode : ReadyTankNode {
            public LifestealGraphicsEffectComponent lifestealGraphicsEffect;

            public LifestealGraphicsEffectReadyComponent lifestealGraphicsEffectReady;
        }
    }
}