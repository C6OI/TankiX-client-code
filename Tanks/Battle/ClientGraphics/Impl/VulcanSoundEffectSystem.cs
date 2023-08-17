using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitEffects(NodeAddedEvent evt, [Combine] VulcanAllSoundEffectsNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            VulcanSoundManagerComponent vulcanSoundManagerComponent = new();
            InitSoundEffect(weapon.vulcanShootingSoundEffect, vulcanSoundManagerComponent, transform);
            InitSoundEffect(weapon.vulcanAfterShootingSoundEffect, vulcanSoundManagerComponent, transform);
            InitSoundEffect(weapon.vulcanChainStartSoundEffect, vulcanSoundManagerComponent, transform);
            InitSoundEffect(weapon.vulcanSlowDownAfterSpeedUpSoundEffect, vulcanSoundManagerComponent, transform);
            InitSoundEffect(weapon.vulcanHitSoundsEffect, vulcanSoundManagerComponent, transform);
            InitSoundEffect(weapon.vulcanTurbineSoundEffect, vulcanSoundManagerComponent, transform);
            float length = weapon.vulcanTurbineSoundEffect.SoundSource.clip.length;
            float speedUpTime = weapon.vulcanWeapon.SpeedUpTime;
            weapon.vulcanTurbineSoundEffect.StartTimePerSec = length - speedUpTime;
            weapon.vulcanTurbineSoundEffect.SoundSource.time = weapon.vulcanTurbineSoundEffect.StartTimePerSec;
            weapon.Entity.AddComponent(vulcanSoundManagerComponent);
        }

        [OnEventFire]
        public void StopCurrentSoundOnIdleState(NodeAddedEvent evt, VulcanIdleStateNode weapon) =>
            StopCurrentSound(weapon.vulcanSoundManager);

        [OnEventFire]
        public void PlaySpeedUpSounds(NodeAddedEvent evt, VulcanSpeedUpNode weapon) {
            AudioSource soundSource = weapon.vulcanTurbineSoundEffect.SoundSource;
            AudioSource soundSource2 = weapon.vulcanChainStartSoundEffect.SoundSource;
            VulcanSoundManagerComponent vulcanSoundManager = weapon.vulcanSoundManager;

            weapon.vulcanSlowDownAfterSpeedUpSoundEffect.AdditionalStartTimeOffset =
                weapon.vulcanWeapon.SpeedUpTime * (1f - weapon.vulcanWeaponState.State);

            PlayNextSound(soundSource, vulcanSoundManager);
            PlaySound(soundSource2, vulcanSoundManager);
        }

        [OnEventFire]
        public void PlayShootingProcessEffect(NodeAddedEvent evt, VulcanShootingNode weapon) {
            AudioSource soundSource = weapon.vulcanShootingSoundEffect.SoundSource;
            PlayNextSound(soundSource, weapon.vulcanSoundManager);
        }

        [OnEventFire]
        public void PlaySoundAfterShooting(NodeRemoveEvent evt, VulcanAfterShootingSoundNode weapon) {
            AudioSource soundSource = weapon.vulcanAfterShootingSoundEffect.SoundSource;
            PlayNextSound(soundSource, weapon.vulcanSoundManager);
        }

        [OnEventFire]
        public void PlaySlowDownSound(NodeAddedEvent evt, VulcanSlowDownNode weapon) {
            if (!weapon.vulcanSlowDown.IsAfterShooting) {
                AudioSource soundSource = weapon.vulcanSlowDownAfterSpeedUpSoundEffect.SoundSource;

                float time = weapon.vulcanSlowDownAfterSpeedUpSoundEffect.StartTimePerSec +
                             weapon.vulcanSlowDownAfterSpeedUpSoundEffect.AdditionalStartTimeOffset;

                soundSource.time = time;
                VulcanFadeSoundBehaviour component = soundSource.gameObject.GetComponent<VulcanFadeSoundBehaviour>();
                component.fadeDuration = weapon.vulcanWeaponState.State * weapon.vulcanWeapon.SlowDownTime;
                component.enabled = true;
                PlayNextSound(soundSource, weapon.vulcanSoundManager);
            }
        }

        [OnEventFire]
        public void StartHitSounds(NodeAddedEvent evt, VulcanHitSoundsNode weapon) {
            UpdateHitSoundsByForce(weapon.vulcanHitSoundsEffect, weapon.streamHit);
            UpdateHitSoundPosition(weapon.vulcanHitSoundsEffect, weapon.streamHit);
        }

        [OnEventComplete]
        public void UpdateHitSound(UpdateEvent evt, VulcanHitSoundsNode weapon) {
            UpdateHitSoundsIfNeeded(weapon.vulcanHitSoundsEffect, weapon.streamHit);
            UpdateHitSoundPosition(weapon.vulcanHitSoundsEffect, weapon.streamHit);
        }

        [OnEventFire]
        public void StopHitSounds(NodeRemoveEvent evt, VulcanHitSoundsNode weapon) {
            weapon.vulcanHitSoundsEffect.SoundSource.Stop();
            weapon.vulcanHitSoundsEffect.SoundSource.gameObject.transform.localPosition = Vector3.zero;
        }

        void UpdateHitSoundPosition(VulcanHitSoundsEffectComponent effect, StreamHitComponent hit) {
            bool flag = hit.StaticHit != null;
            bool flag2 = hit.TankHit != null;

            if (flag) {
                effect.SoundSource.gameObject.transform.position = hit.StaticHit.Position;
            }

            if (flag2) {
                effect.SoundSource.gameObject.transform.position = hit.TankHit.TargetPosition;
            }
        }

        void UpdateHitSoundsIfNeeded(VulcanHitSoundsEffectComponent effect, StreamHitComponent hit) {
            bool flag = hit.StaticHit != null;

            if (effect.IsStaticHit != flag) {
                UpdateHitSoundsByForce(effect, hit);
            }
        }

        void UpdateHitSoundsByForce(VulcanHitSoundsEffectComponent effect, StreamHitComponent hit) {
            bool flag2 = effect.IsStaticHit = hit.StaticHit != null;
            effect.SoundSource.Stop();

            if (flag2) {
                effect.SoundSource.clip = effect.StaticHitClip;
            } else {
                effect.SoundSource.clip = effect.TargetHitClip;
            }

            effect.SoundSource.Play();
        }

        void InitSoundEffect(AbstractVulcanSoundEffectComponent soundEffectComponent,
            VulcanSoundManagerComponent soundManagerComponent, Transform soundRoot) {
            GameObject gameObject = Object.Instantiate(soundEffectComponent.EffectPrefab);
            gameObject.transform.parent = soundRoot;
            gameObject.transform.localPosition = Vector3.zero;
            AudioSource audioSource = soundEffectComponent.SoundSource = gameObject.GetComponent<AudioSource>();
            audioSource.time = soundEffectComponent.StartTimePerSec;
            soundManagerComponent.SoundsWithDelay.Add(audioSource, soundEffectComponent.DelayPerSec);
        }

        void PlayNextSound(AudioSource sound, VulcanSoundManagerComponent manager) {
            StopCurrentSound(manager);
            manager.CurrentSound = sound;
            PlaySound(sound, manager);
        }

        void PlaySound(AudioSource sound, VulcanSoundManagerComponent manager) =>
            sound.PlayDelayed(manager.SoundsWithDelay[sound]);

        void StopCurrentSound(VulcanSoundManagerComponent manager) {
            if (!(manager.CurrentSound == null)) {
                manager.CurrentSound.Stop();
            }
        }

        public class VulcanAllSoundEffectsNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public VulcanAfterShootingSoundEffectComponent vulcanAfterShootingSoundEffect;

            public VulcanChainStartSoundEffectComponent vulcanChainStartSoundEffect;

            public VulcanHitSoundsEffectComponent vulcanHitSoundsEffect;

            public VulcanShootingSoundEffectComponent vulcanShootingSoundEffect;

            public VulcanSlowDownAfterSpeedUpSoundEffectComponent vulcanSlowDownAfterSpeedUpSoundEffect;

            public VulcanTurbineSoundEffectComponent vulcanTurbineSoundEffect;

            public VulcanWeaponComponent vulcanWeapon;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class VulcanShootingNode : Node {
            public VulcanShootingComponent vulcanShooting;

            public VulcanShootingSoundEffectComponent vulcanShootingSoundEffect;

            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanSpeedUpNode : Node {
            public VulcanChainStartSoundEffectComponent vulcanChainStartSoundEffect;

            public VulcanSlowDownAfterSpeedUpSoundEffectComponent vulcanSlowDownAfterSpeedUpSoundEffect;

            public VulcanSoundManagerComponent vulcanSoundManager;

            public VulcanSpeedUpComponent vulcanSpeedUp;

            public VulcanTurbineSoundEffectComponent vulcanTurbineSoundEffect;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanIdleStateNode : Node {
            public VulcanIdleComponent vulcanIdle;

            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanAfterShootingSoundNode : Node {
            public VulcanAfterShootingSoundEffectComponent vulcanAfterShootingSoundEffect;

            public VulcanShootingComponent vulcanShooting;

            public VulcanSoundManagerComponent vulcanSoundManager;
        }

        public class VulcanSlowDownNode : Node {
            public VulcanSlowDownComponent vulcanSlowDown;

            public VulcanSlowDownAfterSpeedUpSoundEffectComponent vulcanSlowDownAfterSpeedUpSoundEffect;

            public VulcanSoundManagerComponent vulcanSoundManager;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanHitSoundsNode : Node {
            public StreamHitComponent streamHit;
            public VulcanHitSoundsEffectComponent vulcanHitSoundsEffect;

            public VulcanSoundManagerComponent vulcanSoundManager;
        }
    }
}