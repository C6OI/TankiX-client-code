using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftShotSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitShaftShotSoundEffects(NodeAddedEvent evt, [Combine] InitialShaftShotSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            weapon.shaftQuickShotSoundEffect.Init(transform);
            weapon.shaftAimingShotSoundEffect.Init(transform);
            weapon.shaftStartCooldownSoundEffect.Init(transform);
            weapon.shaftClosingCooldownSoundEffect.Init(transform);
            float cooldownAnimationTime = weapon.shaftShotAnimation.CooldownAnimationTime;
            ShaftRollCooldownSoundEffectComponent shaftRollCooldownSoundEffect = weapon.shaftRollCooldownSoundEffect;
            shaftRollCooldownSoundEffect.Init(transform);
            shaftRollCooldownSoundEffect.SetFadeOutTime(cooldownAnimationTime);
            weapon.Entity.AddComponent<ShaftShotSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void StopShaftShotSoundEffects(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyShaftShotSoundEffectNode weapon) {
            weapon.shaftAimingShotSoundEffect.Stop();
            weapon.shaftQuickShotSoundEffect.Stop();
            StopCooldownSounds(weapon);
        }

        [OnEventFire]
        public void PlayShaftQuickShotEffect(BaseShotEvent evt, ReadyShaftShotSoundEffectNode weapon, [JoinByTank] SingleNode<ShaftIdleStateComponent> state,
            [JoinByTank] ActiveTankNode tank) {
            StopCooldownSounds(weapon);
            weapon.shaftAimingShotSoundEffect.Stop();
            weapon.shaftQuickShotSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayShaftAimingShotEffect(BaseShotEvent evt, ReadyShaftShotSoundEffectNode weapon, [JoinByTank] SingleNode<ShaftAimingWorkFinishStateComponent> state,
            [JoinByTank] ActiveTankNode tank) {
            StopCooldownSounds(weapon);
            weapon.shaftQuickShotSoundEffect.Stop();
            weapon.shaftAimingShotSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayShaftStartCooldownEffect(ShaftShotAnimationCooldownStartEvent evt, ReadyShaftShotSoundEffectNode weapon) {
            weapon.shaftClosingCooldownSoundEffect.Stop();
            weapon.shaftStartCooldownSoundEffect.Play();
            weapon.shaftRollCooldownSoundEffect.Play();
        }

        [OnEventFire]
        public void PlayShaftClosingCooldownEffect(ShaftShotAnimationCooldownClosingEvent evt, ReadyShaftShotSoundEffectNode weapon) {
            weapon.shaftStartCooldownSoundEffect.Stop();
            weapon.shaftClosingCooldownSoundEffect.Play();
        }

        void StopCooldownSounds(ReadyShaftShotSoundEffectNode weapon) {
            weapon.shaftStartCooldownSoundEffect.Stop();
            weapon.shaftClosingCooldownSoundEffect.Stop();
            weapon.shaftRollCooldownSoundEffect.Stop();
        }

        public class InitialShaftShotSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public ShaftAimingShotSoundEffectComponent shaftAimingShotSoundEffect;

            public ShaftClosingCooldownSoundEffectComponent shaftClosingCooldownSoundEffect;

            public ShaftQuickShotSoundEffectComponent shaftQuickShotSoundEffect;

            public ShaftRollCooldownSoundEffectComponent shaftRollCooldownSoundEffect;

            public ShaftShotAnimationComponent shaftShotAnimation;

            public ShaftShotAnimationESMComponent shaftShotAnimationEsm;

            public ShaftStartCooldownSoundEffectComponent shaftStartCooldownSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ReadyShaftShotSoundEffectNode : InitialShaftShotSoundEffectNode {
            public ShaftShotSoundEffectReadyComponent shaftShotSoundEffectReady;
        }

        public class ActiveTankNode : Node {
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}