using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftShotAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitShaftShotAnimation(NodeAddedEvent evt, InitialShaftShotAnimationNode weapon) {
            Animator animator = weapon.animation.Animator;
            ShaftEnergyComponent shaftEnergy = weapon.shaftEnergy;
            float reloadEnergyPerSec = shaftEnergy.ReloadEnergyPerSec;
            float unloadEnergyPerQuickShot = shaftEnergy.UnloadEnergyPerQuickShot;
            float cooldownIntervalSec = weapon.weaponCooldown.CooldownIntervalSec;
            float possibleUnloadEnergyPerAimingShot = shaftEnergy.PossibleUnloadEnergyPerAimingShot;
            Entity entity = weapon.Entity;
            weapon.shaftShotAnimation.Init(animator, cooldownIntervalSec, unloadEnergyPerQuickShot, reloadEnergyPerSec, possibleUnloadEnergyPerAimingShot);
            weapon.shaftShotAnimationTrigger.Entity = entity;
            ShaftShotAnimationESMComponent shaftShotAnimationESMComponent = new();
            entity.AddComponent(shaftShotAnimationESMComponent);
            EntityStateMachine esm = shaftShotAnimationESMComponent.Esm;
            esm.AddState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
            esm.AddState<ShaftShotAnimationStates.ShaftShotAnimationBounceState>();
            esm.AddState<ShaftShotAnimationStates.ShaftShotAnimationCooldownState>();
            esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
        }

        [OnEventFire]
        public void PlayShot(BaseShotEvent evt, ReadyShaftShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank) {
            weapon.shaftShotAnimation.PlayShot();
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationBounceState>();
        }

        [OnEventFire]
        public void StartCooldown(ShaftShotAnimationCooldownStartEvent evt, ReadyShaftShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank) {
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationCooldownState>();
        }

        [OnEventFire]
        public void UpdateShotAnimationDataWithinCooldown(UpdateEvent evt, ShaftShotAnimationCooldownStateNode state, [JoinByTank] ReadyShaftShotAnimationNode weapon) {
            weapon.shaftShotAnimation.UpdateShotCooldownAnimation(evt.DeltaTime);
        }

        [OnEventFire]
        public void StopCooldown(ShaftShotAnimationCooldownEndEvent evt, ReadyShaftShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank) {
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
        }

        [OnEventFire]
        public void StopShaftShotAnimtionEffect(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyShaftShotAnimationNode weapon) {
            weapon.shaftShotAnimationEsm.Esm.ChangeState<ShaftShotAnimationStates.ShaftShotAnimationIdleState>();
        }

        public class InitialShaftShotAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public ShaftEnergyComponent shaftEnergy;

            public ShaftShotAnimationComponent shaftShotAnimation;

            public ShaftShotAnimationTriggerComponent shaftShotAnimationTrigger;

            public TankGroupComponent tankGroup;

            public WeaponCooldownComponent weaponCooldown;
        }

        public class ReadyShaftShotAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public ShaftShotAnimationComponent shaftShotAnimation;

            public ShaftShotAnimationESMComponent shaftShotAnimationEsm;

            public TankGroupComponent tankGroup;
        }

        public class ShaftShotAnimationCooldownStateNode : Node {
            public ShaftShotAnimationCooldownStateComponent shaftShotAnimationCooldownState;

            public TankGroupComponent tankGroup;
        }

        public class ActiveTankNode : Node {
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}