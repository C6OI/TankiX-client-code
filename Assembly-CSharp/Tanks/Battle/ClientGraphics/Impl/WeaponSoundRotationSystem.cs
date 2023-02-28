using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class WeaponSoundRotationSystem : ECSSystem {
        [OnEventFire]
        public void CreateWeaponRotationSound(NodeAddedEvent evt, [Combine] WeaponSoundNode weaponNode, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            WeaponRotationSoundComponent weaponRotationSound = weaponNode.weaponRotationSound;
            GameObject gameObject = Object.Instantiate(weaponRotationSound.Asset);
            gameObject.transform.parent = weaponNode.weaponSoundRoot.gameObject.transform;
            gameObject.transform.localPosition = Vector3.zero;
            WeaponRotationSoundBehaviour component = gameObject.GetComponent<WeaponRotationSoundBehaviour>();
            weaponRotationSound.StartAudioSource = component.startAudioSource;
            weaponRotationSound.LoopAudioSource = component.loopAudioSource;
            weaponRotationSound.StopAudioSource = component.stopAudioSource;
            weaponRotationSound.IsActive = false;
            weaponNode.Entity.AddComponent<WeaponRotationSoundReadyComponent>();
        }

        [OnEventComplete]
        public void UpdateWeaponRotationSound(UpdateEvent evt, ReadyWeaponSoundRotationNode weapon, [JoinByTank] ActiveTankNode activeTank) {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;

            if (weaponRotationControl.IsRotating()) {
                StartAudioSources(weapon.weaponRotationSound);
            } else {
                StopAudioSources(weapon.weaponRotationSound);
            }
        }

        [OnEventFire]
        public void StopWeaponRotationSound(NodeRemoveEvent evt, ActiveTankNode activeTank, [JoinByTank] ReadyWeaponSoundRotationNode weapon) {
            StopAudioSources(weapon.weaponRotationSound);
        }

        [OnEventFire]
        public void StopWeaponRotationSound(NodeRemoveEvent evt, ReadyWeaponSoundRotationNode weapon) {
            StopAudioSources(weapon.weaponRotationSound);
        }

        void StartAudioSources(WeaponRotationSoundComponent sounds) {
            if (!sounds.IsActive) {
                sounds.StopAudioSource.Stop();
                sounds.StartAudioSource.Play();
                double time = AudioSettings.dspTime + sounds.StartAudioSource.clip.length;
                sounds.LoopAudioSource.PlayScheduled(time);
                sounds.IsActive = true;
            }
        }

        void StopAudioSources(WeaponRotationSoundComponent sounds) {
            if (sounds.IsActive) {
                sounds.StartAudioSource.Stop();
                sounds.LoopAudioSource.Stop();
                sounds.StopAudioSource.Play();
                sounds.IsActive = false;
            }
        }

        [Not(typeof(TankAutopilotComponent))]
        public class ActiveTankNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;
        }

        public class WeaponSoundNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public TankGroupComponent tankGroup;

            public WeaponRotationSoundComponent weaponRotationSound;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ReadyWeaponSoundRotationNode : WeaponSoundNode {
            public WeaponRotationComponent weaponRotation;

            public WeaponRotationControlComponent weaponRotationControl;

            public WeaponRotationSoundReadyComponent weaponRotationSoundReady;
        }
    }
}