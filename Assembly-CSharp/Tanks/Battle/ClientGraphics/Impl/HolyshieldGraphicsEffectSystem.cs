using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HolyshieldGraphicsEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitTankHolyshieldEffect(NodeAddedEvent e, TankHolyshieldGraphicsEffectNode tank, [JoinAll] Optional<SingleNode<SelfTankComponent>> selfTank) {
            GameObject gameObject = tank.newHolyshieldEffect.InitEffect(tank.tankVisualRoot.transform, tank.baseRenderer.Renderer as SkinnedMeshRenderer, Layers.VISUAL_STATIC);
            gameObject.AddComponent<Rigidbody>().isKinematic = true;

            if (selfTank.IsPresent()) {
                gameObject.AddComponent<HolyshiedTargetBehaviour>().Init(selfTank.Get().Entity);
            }

            tank.Entity.AddComponent<TankHolyshieldEffectReadyComponent>();
        }

        [OnEventFire]
        public void Play(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank] [Context] TankReadyHolyshieldGraphicsEffectNode tank) {
            tank.newHolyshieldEffect.Play();
        }

        [OnEventFire]
        public void Stop(NodeRemoveEvent e, HolyshieldEffectNode effect, [JoinByTank] TankReadyHolyshieldGraphicsEffectNode tank) {
            tank.newHolyshieldEffect.Stop();
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank] [Context] SelfTankNode tank, [JoinByTank] [Context] IdleShaftNode shaft) {
            tank.newHolyshieldEffect.UpdateAlpha(1f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank] [Context] SelfTankNode tank, [JoinByTank] [Context] WaitingShaftNode shaft) {
            tank.newHolyshieldEffect.UpdateAlpha(1f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank] [Context] SelfTankNode tank, [JoinByTank] [Context] ShaftAimingWorkingNode shaft) {
            tank.newHolyshieldEffect.UpdateAlpha(0f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank] [Context] SelfTankNode tank,
            [JoinByTank] [Context] ShaftAimingWorkActivationNode shaft) {
            tank.newHolyshieldEffect.UpdateAlpha(1f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank] [Context] SelfTankNode tank,
            [JoinByTank] [Context] ShaftAimingWorkFinishNode shaft) {
            tank.newHolyshieldEffect.UpdateAlpha(0f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(UpdateEvent e, HolyshieldEffectNode effect, [JoinByTank] SelfTankNode tank, [JoinByTank] ShaftAimingWorkActivationNode shaft) {
            float num = Mathf.Clamp01(shaft.shaftAimingWorkActivationState.ActivationTimer / shaft.shaftStateConfig.ActivationToWorkingTransitionTimeSec);
            tank.newHolyshieldEffect.UpdateAlpha(1f - num);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(UpdateEvent e, HolyshieldEffectNode effect, [JoinByTank] SelfTankNode tank, [JoinByTank] ShaftAimingWorkFinishNode shaft) {
            float alpha = Mathf.Clamp01(shaft.shaftAimingWorkFinishState.FinishTimer / shaft.shaftStateConfig.FinishToIdleTransitionTimeSec);
            tank.newHolyshieldEffect.UpdateAlpha(alpha);
        }

        public class TankHolyshieldGraphicsEffectNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;

            public NewHolyshieldEffectComponent newHolyshieldEffect;

            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;

            public VisualMountPointComponent visualMountPoint;
        }

        public class TankReadyHolyshieldGraphicsEffectNode : TankHolyshieldGraphicsEffectNode {
            public TankHolyshieldEffectReadyComponent tankHolyshieldEffectReady;
        }

        public class SelfTankNode : TankReadyHolyshieldGraphicsEffectNode {
            public SelfTankComponent selfTank;
        }

        public class HolyshieldEffectNode : Node {
            public HolyshieldEffectComponent holyshieldEffect;
            public InvulnerabilityEffectComponent invulnerabilityEffect;

            public TankGroupComponent tankGroup;
        }

        public class ShaftNode : Node {
            public ShaftComponent shaft;

            public ShaftStateConfigComponent shaftStateConfig;
            public TankGroupComponent tankGroup;
        }

        public class IdleShaftNode : ShaftNode {
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class WaitingShaftNode : ShaftNode {
            public ShaftWaitingStateComponent shaftWaitingState;
        }

        public class ShaftAimingWorkActivationNode : ShaftNode {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
        }

        public class ShaftAimingWorkingNode : ShaftNode {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class ShaftAimingWorkFinishNode : ShaftNode {
            public ShaftAimingWorkFinishStateComponent shaftAimingWorkFinishState;
        }
    }
}