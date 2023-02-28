using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankInvisibilityEffectSystem : ECSSystem {
        [OnEventFire]
        public void ActivateTankInvisibilityGraphicEffect(NodeAddedEvent e, InvisibilityEffectNode effect, [Context] [JoinByTank] InitializedTankInvisibilityEffectNode tank) {
            ScheduleEvent<ActivateTankInvisibilityEffectEvent>(tank);
        }

        [OnEventFire]
        public void DeactivateTankInvisibilityGraphicEffect(NodeRemoveEvent e, InvisibilityEffectNode effect, [JoinByTank] InitializedTankInvisibilityEffectNode tank) {
            ScheduleEvent<DeactivateTankInvisibilityEffectEvent>(tank);
        }

        [OnEventFire]
        public void InitTankVisibilityEffectStates(NodeAddedEvent evt, TankInvisibilityEffectNode tank, [JoinByTank] [Context] WeaponNode weapon) {
            TankInvisibilityEffectESMComponent tankInvisibilityEffectESMComponent = new();
            tank.Entity.AddComponent(tankInvisibilityEffectESMComponent);
            tankInvisibilityEffectESMComponent.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectActivationState>();
            tankInvisibilityEffectESMComponent.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectDeactivationState>();
            tankInvisibilityEffectESMComponent.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectIdleState>();
            tankInvisibilityEffectESMComponent.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectWorkingState>();
            bool fullInvisibly = tank.Entity.HasComponent<EnemyComponent>();
            tank.tankInvisibilityEffectUnity.ConfigureEffect(tank.Entity, fullInvisibly, tank.baseRenderer.Renderer, weapon.baseRenderer.Renderer);
            tank.tankInvisibilityEffectUnity.ResetEffect();
            tank.Entity.AddComponent<TankInvisibilityEffectInitializedComponent>();
        }

        [OnEventFire]
        public void ResetEffectOnTankIncarnation(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] InitializedTankInvisibilityEffectNode tank) {
            tank.tankInvisibilityEffectUnity.ResetEffect();
        }

        [OnEventFire]
        public void SwitchTankInvisibilityEffectState(TankInvisibilityEffectSwitchStateEvent e, SingleNode<TankInvisibilityEffectESMComponent> tank) {
            tank.component.Esm.ChangeState(e.StateType);
        }

        [OnEventFire]
        public void ActivateInvisibilityEffect(ActivateTankInvisibilityEffectEvent evt, IdleTankInvisibilityEffectNode tank) {
            ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.TANK_INVISIBILITY_EFFECT), tank);
            tank.tankInvisibilityEffectUnity.ActivateEffect();
        }

        [OnEventFire]
        public void ActivateInvisibilityEffect(ActivateTankInvisibilityEffectEvent evt, DeactivationTankInvisibilityEffectNode tank) {
            ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.TANK_INVISIBILITY_EFFECT), tank);
            tank.tankInvisibilityEffectUnity.ActivateEffect();
        }

        [OnEventFire]
        public void DeactivateInvisibilityEffect(DeactivateTankInvisibilityEffectEvent evt, WorkingTankInvisibilityEffectNode tank) {
            tank.tankInvisibilityEffectUnity.DeactivateEffect();
        }

        [OnEventFire]
        public void DeactivateInvisibilityEffect(DeactivateTankInvisibilityEffectEvent evt, ActivationTankInvisibilityEffectNode tank) {
            tank.tankInvisibilityEffectUnity.DeactivateEffect();
        }

        [OnEventFire]
        public void RemoveShaderEffect(NodeAddedEvent e, SingleNode<TankInvisibilityEffectIdleStateComponent> tank) {
            ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.TANK_INVISIBILITY_EFFECT, false), tank);
        }

        public class InvisibilityEffectNode : Node {
            public InvisibilityEffectComponent invisibilityEffect;

            public TankGroupComponent tankGroup;
        }

        public class TankInvisibilityEffectNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;

            public TankInvisibilityEffectUnityComponent tankInvisibilityEffectUnity;

            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;

            public TankVisualRootComponent tankVisualRoot;
        }

        public class WeaponNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankGroupComponent tankGroup;

            public WeaponComponent weapon;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }

        public class InitializedTankInvisibilityEffectNode : TankInvisibilityEffectNode {
            public TankInvisibilityEffectESMComponent tankInvisibilityEffectEsm;

            public TankInvisibilityEffectInitializedComponent tankInvisibilityEffectInitialized;
        }

        public class IdleTankInvisibilityEffectNode : InitializedTankInvisibilityEffectNode {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class WorkingTankInvisibilityEffectNode : InitializedTankInvisibilityEffectNode {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class ActivationTankInvisibilityEffectNode : InitializedTankInvisibilityEffectNode {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class DeactivationTankInvisibilityEffectNode : InitializedTankInvisibilityEffectNode {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }
    }
}