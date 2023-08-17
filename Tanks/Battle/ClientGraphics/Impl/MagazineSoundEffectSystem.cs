using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MagazineSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitMagazineSounds(NodeAddedEvent evt, [Combine] InitialMagazineSoundEffectNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.gameObject.transform;
            PrepareMagazineSoundEffect(weapon.magazineLastCartridgeChargeEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineBlowOffEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineOffsetEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineRollEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineCartridgeClickEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineShotEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineBounceEffect, transform);
            PrepareMagazineSoundEffect(weapon.magazineCooldownEffect, transform);
            weapon.Entity.AddComponent<MagazineSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void StopSoundPlay(NodeRemoveEvent evt, ActiveTankNode tank,
            [JoinByTank] ReadyMagazineSoundEffectNode weapon) {
            StopPlaying(weapon.magazineLastCartridgeChargeEffect);
            StopPlaying(weapon.magazineBlowOffEffect);
            StopPlaying(weapon.magazineOffsetEffect);
            StopPlaying(weapon.magazineRollEffect);
            StopPlaying(weapon.magazineCartridgeClickEffect);
            StopPlaying(weapon.magazineShotEffect);
            StopPlaying(weapon.magazineBounceEffect);
            StopPlaying(weapon.magazineCooldownEffect);
        }

        [OnEventFire]
        public void PlayCooldownEffect(HammerCooldownEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineCooldownEffect);

        [OnEventFire]
        public void PlayShotEffect(HammerMagazineShotEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineShotEffect);

        [OnEventFire]
        public void PlayBounceEffect(HammerBounceEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineBounceEffect);

        [OnEventFire]
        public void PlayLastCartridgeChargeEffect(HammerChargeLastCartridgeEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineLastCartridgeChargeEffect);

        [OnEventFire]
        public void PlayBlowOffEffect(HammerBlowOffEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineBlowOffEffect);

        [OnEventFire]
        public void PlayOffsetEffect(HammerOffsetEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineOffsetEffect);

        [OnEventFire]
        public void PlayRollEffect(HammerRollEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineRollEffect);

        [OnEventFire]
        public void PlayClickEffect(HammerCartridgeClickEvent evt, ReadyMagazineSoundEffectNode weapon,
            [JoinByTank] ActiveTankNode tank) => PlaySoundEffect(weapon.magazineCartridgeClickEffect);

        void PlaySoundEffect(MagazineSoundEffectComponent soundEffect) {
            AudioSource audioSource = soundEffect.AudioSource;

            if (audioSource.isPlaying) {
                audioSource.Stop();
            }

            audioSource.Play();
        }

        void StopPlaying(MagazineSoundEffectComponent soundEffect) {
            if (soundEffect.AudioSource.isPlaying) {
                soundEffect.AudioSource.Stop();
            }
        }

        void PrepareMagazineSoundEffect(MagazineSoundEffectComponent magazineSoundEffect, Transform root) {
            GameObject asset = magazineSoundEffect.Asset;
            AudioSource audioSource = InstantiateAudioEffect(asset, root);
            magazineSoundEffect.AudioSource = audioSource;
        }

        AudioSource InstantiateAudioEffect(GameObject prefab, Transform root) {
            GameObject gameObject = Object.Instantiate(prefab);
            gameObject.transform.parent = root;
            gameObject.transform.localPosition = Vector3.zero;
            return gameObject.GetComponent<AudioSource>();
        }

        public class InitialMagazineSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public HammerShotAnimationComponent hammerShotAnimation;

            public HammerShotAnimationReadyComponent hammerShotAnimationReady;

            public MagazineBlowOffEffectComponent magazineBlowOffEffect;

            public MagazineBounceEffectComponent magazineBounceEffect;

            public MagazineCartridgeClickEffectComponent magazineCartridgeClickEffect;

            public MagazineCooldownEffectComponent magazineCooldownEffect;

            public MagazineLastCartridgeChargeEffectComponent magazineLastCartridgeChargeEffect;

            public MagazineOffsetEffectComponent magazineOffsetEffect;

            public MagazineRollEffectComponent magazineRollEffect;

            public MagazineShotEffectComponent magazineShotEffect;

            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;

            public WeaponCooldownComponent weaponCooldown;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ReadyMagazineSoundEffectNode : InitialMagazineSoundEffectNode {
            public MagazineSoundEffectReadyComponent magazineSoundEffectReady;
        }

        public class ActiveTankNode : Node {
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}