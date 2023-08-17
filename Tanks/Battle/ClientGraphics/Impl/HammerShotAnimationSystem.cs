using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HammerShotAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitHammerShotAnimation(NodeAddedEvent evt, InitialHammerShotAnimationNode weapon) {
            Animator animator = weapon.animation.Animator;
            float cooldownIntervalSec = weapon.weaponCooldown.CooldownIntervalSec;
            Entity entity = weapon.Entity;

            weapon.hammerShotAnimation.InitHammerShotAnimation(entity,
                animator,
                weapon.magazineWeapon.ReloadMagazineTimePerSec,
                cooldownIntervalSec);

            entity.AddComponent<HammerShotAnimationReadyComponent>();
        }

        [OnEventFire]
        public void PlayShot(BaseShotEvent evt, ReadyHammerShotAnimationNode weapon, [JoinByTank] ActiveTankNode tank) {
            if (weapon.magazineLocalStorage.CurrentCartridgeCount > 1) {
                weapon.hammerShotAnimation.PlayShot();
            } else {
                weapon.hammerShotAnimation.PlayShotAndReload();
            }
        }

        [OnEventComplete]
        public void Reset(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyHammerShotAnimationNode weapon) {
            weapon.Entity.RemoveComponent<HammerShotAnimationReadyComponent>();
            weapon.hammerShotAnimation.Reset();
        }

        public class InitialHammerShotAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public HammerShotAnimationComponent hammerShotAnimation;

            public MagazineLocalStorageComponent magazineLocalStorage;

            public MagazineStorageComponent magazineStorage;
            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;

            public WeaponCooldownComponent weaponCooldown;
        }

        public class ReadyHammerShotAnimationNode : Node {
            public HammerShotAnimationComponent hammerShotAnimation;

            public HammerShotAnimationReadyComponent hammerShotAnimationReady;

            public MagazineLocalStorageComponent magazineLocalStorage;

            public MagazineStorageComponent magazineStorage;
            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}