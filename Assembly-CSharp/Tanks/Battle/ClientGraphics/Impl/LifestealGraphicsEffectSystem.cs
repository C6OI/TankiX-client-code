using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class LifestealGraphicsEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitLifestealEffect(NodeAddedEvent e, TankNode tank, [JoinByTank] [Context] WeaponNode weapon) {
            tank.Entity.RemoveComponentIfPresent<LifestealGraphicsEffectReadyComponent>();

            tank.lifestealGraphicsEffect.InitRepairGraphicsEffect(new HealingGraphicEffectInputs(tank.Entity, tank.baseRenderer.Renderer as SkinnedMeshRenderer),
                new WeaponHealingGraphicEffectInputs(weapon.Entity, weapon.weaponVisualRoot.transform, weapon.baseRenderer.Renderer as SkinnedMeshRenderer),
                tank.tankSoundRoot.SoundRootTransform, tank.visualMountPoint.MountPoint);

            tank.Entity.AddComponent<LifestealGraphicsEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayLifestealEffect(TriggerEffectExecuteEvent e, SingleNode<LifestealEffectComponent> effect, [JoinByTank] ActiveTankReadyNode tank) {
            ScheduleEvent<PlayLifestealGraphicsEffectEvent>(tank);
        }

        [OnEventFire]
        public void PrepareLifeStealEffect(PlayLifestealGraphicsEffectEvent evt, ActiveTankReadyNode tank) {
            ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.LIFESTEAL_EFFECT), tank);
        }

        [OnEventFire]
        public void StopHealingGraphicEfect(PlayLifestealGraphicsEffectEvent evt, HealingTankReadyNode tank) {
            tank.healingGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopEmergencyProtectionGraphicEfect(PlayLifestealGraphicsEffectEvent evt, EmergencyProtectionTankReadyNode tank) {
            tank.simpleEmergencyProtectionGraphicEffect.StopEffect();
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectIdleStateNode tank) {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankShader.TransparentShader);
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectWorkingStateNode tank) {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectShader);
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectActivationStateNode tank) {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader);
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectDeactivationStateNode tank) {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader);
        }

        [OnEventFire]
        public void StopLifestealGraphicsEffect(NodeRemoveEvent e, ActiveTankReadyNode tank) {
            tank.lifestealGraphicsEffect.StopEffect();
        }

        [OnEventFire]
        public void StopLifestealGraphicsEffect(StopLifestealTankShaderEffectEvent e, SingleNode<TankComponent> tank) {
            ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.LIFESTEAL_EFFECT, false), tank);
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;

            public HealingGraphicsEffectReadyComponent healingGraphicsEffectReady;

            public LifestealGraphicsEffectComponent lifestealGraphicsEffect;

            public RendererInitializedComponent rendererInitialized;
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TankInvisibilityEffectUnityComponent tankInvisibilityEffectUnity;

            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;

            public TankShaderComponent tankShader;

            public TankSoundRootComponent tankSoundRoot;

            public TankVisualRootComponent tankVisualRoot;

            public VisualMountPointComponent visualMountPoint;
        }

        public class TankReadyNode : TankNode {
            public LifestealGraphicsEffectReadyComponent lifestealGraphicsEffectReady;
        }

        public class HealingTankReadyNode : TankReadyNode {
            public HealingGraphicEffectComponent healingGraphicEffect;
        }

        public class EmergencyProtectionTankReadyNode : TankReadyNode {
            public EmergencyProtectionTankShaderEffectReadyComponent emergencyProtectionTankShaderEffectReady;
            public SimpleEmergencyProtectionGraphicEffectComponent simpleEmergencyProtectionGraphicEffect;
        }

        public class ActiveTankReadyNode : TankReadyNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class ReadyTankInvisibilityEffectIdleStateNode : ActiveTankReadyNode {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class ReadyTankInvisibilityEffectWorkingStateNode : ActiveTankReadyNode {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class ReadyTankInvisibilityEffectActivationStateNode : ActiveTankReadyNode {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class ReadyTankInvisibilityEffectDeactivationStateNode : ActiveTankReadyNode {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class WeaponNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererInitializedComponent rendererInitialized;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class PlayLifestealGraphicsEffectEvent : Event { }
    }
}