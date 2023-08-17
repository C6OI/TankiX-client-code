using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InstantiateAimingSoundsEffectsForSelfWeapon(NodeAddedEvent evt,
            [Combine] InitialSelfShaftAimingSoundsEffectNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            weapon.shaftAimingControllerSoundEffect.Init(transform);
            weapon.Entity.AddComponent<ShaftSelfAimingSoundsInstantiatedComponent>();
        }

        [OnEventFire]
        public void InstantiateAimingSoundsEffectsForRemoteWeapon(NodeAddedEvent evt,
            [Combine] InitialRemoteShaftAimingSoundsEffectNode weapon, [Context] [JoinByTank] [Combine] RemoteTankNode tank,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            weapon.shaftAimingOptixMovementSoundEffect.Init(transform);
            weapon.Entity.AddComponent<ShaftRemoteAimingSoundsInstantiatedComponent>();
        }

        [OnEventFire]
        public void InstantiateAimingSoundsEffectsForAnyWeapon(NodeAddedEvent evt,
            [Combine] InitialAnyShaftAimingSoundsEffectNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect = weapon.shaftStartAimingSoundEffect;
            ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect = weapon.shaftAimingLoopSoundEffect;
            shaftStartAimingSoundEffect.Init(transform);
            shaftAimingLoopSoundEffect.Init(transform);
            float startAimingDurationSec = shaftStartAimingSoundEffect.StartAimingDurationSec;
            shaftAimingLoopSoundEffect.SetDelay(startAimingDurationSec);
            weapon.Entity.AddComponent<ShaftAnyAimingSoundsInstantiatedComponent>();
        }

        [OnEventFire]
        public void StartOptixMovementEffect(NodeAddedEvent evt, ShaftAimingWorkingStateSoundEffectNode weapon) =>
            weapon.shaftAimingOptixMovementSoundEffect.Play();

        [OnEventFire]
        public void StopOptixMovementEffect(NodeRemoveEvent evt, ShaftAimingWorkingStateSoundEffectNode weapon) =>
            weapon.shaftAimingOptixMovementSoundEffect.Stop();

        [OnEventFire]
        public void StartTargetingControllerSound(NodeAddedEvent evt, ShaftAimingControllerSoundEffectWorkingNode weapon) =>
            weapon.Entity.AddComponent<ShaftAimingControllerPlayingComponent>();

        [OnEventFire]
        public void PlayTargetingControllerSound(UpdateEvent evt,
            ShaftAimingControllerSoundEffectPlayingWorkingStateNode weapon) {
            if (weapon.shaftAimingWorkingState.IsActive) {
                weapon.shaftAimingControllerSoundEffect.Play();
            } else {
                weapon.shaftAimingControllerSoundEffect.Stop();
            }
        }

        [OnEventFire]
        public void StopPlayingTargetingControllerSound(NodeRemoveEvent evt,
            ShaftAimingControllerSoundEffectPlayingNode weapon) => weapon.shaftAimingControllerSoundEffect.Stop();

        [OnEventFire]
        public void
            StopPlayingTargetingControllerSound(NodeAddedEvent evt, ShaftAimingControllerSoundEffectIdleNode weapon) =>
            weapon.Entity.RemoveComponent<ShaftAimingControllerPlayingComponent>();

        [OnEventFire]
        public void PlayAimingSounds(NodeAddedEvent evt, ShaftStartLoopAimingSoundsWorkActivationNode weapon) {
            weapon.shaftStartAimingSoundEffect.Play();
            weapon.shaftAimingLoopSoundEffect.Play();
        }

        [OnEventFire]
        public void StopAimingSounds(NodeRemoveEvent evt, ShaftStartLoopAimingSoundsWorkingNode weapon) {
            weapon.shaftStartAimingSoundEffect.Stop();
            weapon.shaftAimingLoopSoundEffect.Stop();
        }

        [OnEventFire]
        public void StopAimingSounds(NodeAddedEvent evt, ShaftStartLoopAimingSoundsIdleNode weapon) {
            weapon.shaftStartAimingSoundEffect.Stop();
            weapon.shaftAimingLoopSoundEffect.Stop();
        }

        [OnEventFire]
        public void DisableWeaponRotationSound(NodeAddedEvent evt, ShaftAimingWorkActivationWeaponRotationSoundNode weapon) {
            weapon.Entity.RemoveComponent<WeaponRotationSoundReadyComponent>();
            weapon.Entity.AddComponent<ShaftAimingRotationSoundReadyStateComponent>();
        }

        [OnEventFire]
        public void EnableWeaponRotationSound(NodeAddedEvent evt, ShaftIdleWeaponRotationSoundNode weapon) {
            weapon.Entity.RemoveComponent<ShaftAimingRotationSoundReadyStateComponent>();
            weapon.Entity.AddComponent<WeaponRotationSoundReadyComponent>();
        }

        public class RemoteTankNode : Node {
            public RemoteTankComponent remoteTank;

            public TankGroupComponent tankGroup;
        }

        public class InitialAnyShaftAimingSoundsEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;

            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class InitialRemoteShaftAimingSoundsEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public ShaftAimingOptixMovementSoundEffectComponent shaftAimingOptixMovementSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class InitialSelfShaftAimingSoundsEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;

            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ShaftAimingWorkActivationWeaponRotationSoundNode : Node {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public WeaponRotationSoundReadyComponent weaponRotationSoundReady;
        }

        public class ShaftIdleWeaponRotationSoundNode : Node {
            public ShaftAimingRotationSoundReadyStateComponent shaftAimingRotationSoundReadyState;
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class ShaftAimingControllerSoundEffectPlayingNode : Node {
            public ShaftAimingControllerPlayingComponent shaftAimingControllerPlaying;
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;

            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
        }

        public class ShaftAimingControllerSoundEffectPlayingWorkingStateNode : Node {
            public ShaftAimingControllerPlayingComponent shaftAimingControllerPlaying;
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;

            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponRotationComponent weaponRotation;
        }

        public class ShaftAimingControllerSoundEffectIdleNode : Node {
            public ShaftAimingControllerPlayingComponent shaftAimingControllerPlaying;
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;

            public ShaftIdleStateComponent shaftIdleState;

            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
        }

        public class ShaftAimingControllerSoundEffectWorkingNode : Node {
            public ShaftAimingControllerSoundEffectComponent shaftAimingControllerSoundEffect;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftSelfAimingSoundsInstantiatedComponent shaftSelfAimingSoundsInstantiated;
        }

        public class ShaftAimingWorkingStateSoundEffectNode : Node {
            public ShaftAimingOptixMovementSoundEffectComponent shaftAimingOptixMovementSoundEffect;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftRemoteAimingSoundsInstantiatedComponent shaftRemoteAimingSoundsInstantiated;
        }

        public class ShaftStartLoopAimingSoundsWorkActivationNode : Node {
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;

            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftAnyAimingSoundsInstantiatedComponent shaftAnyAimingSoundsInstantiated;
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
        }

        public class ShaftStartLoopAimingSoundsWorkingNode : Node {
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftAnyAimingSoundsInstantiatedComponent shaftAnyAimingSoundsInstantiated;
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
        }

        public class ShaftStartLoopAimingSoundsIdleNode : Node {
            public ShaftAimingLoopSoundEffectComponent shaftAimingLoopSoundEffect;

            public ShaftAnyAimingSoundsInstantiatedComponent shaftAnyAimingSoundsInstantiated;

            public ShaftIdleStateComponent shaftIdleState;
            public ShaftStartAimingSoundEffectComponent shaftStartAimingSoundEffect;
        }
    }
}