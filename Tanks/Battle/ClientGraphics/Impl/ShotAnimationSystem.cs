using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShotAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitShotAnimation(NodeAddedEvent e, InitialShotAnimationNode weapon) {
            Animator animator = weapon.animation.Animator;
            float cooldownIntervalSec = weapon.weaponCooldown.CooldownIntervalSec;
            float unloadEnergyPerShot = weapon.discreteWeaponEnergy.UnloadEnergyPerShot;
            float reloadEnergyPerSec = weapon.discreteWeaponEnergy.ReloadEnergyPerSec;
            weapon.shotAnimation.Init(animator, cooldownIntervalSec, unloadEnergyPerShot, reloadEnergyPerSec);
            weapon.Entity.AddComponent<ShotAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartShotAnimation(BaseShotEvent evt, ReadyShotAnimationNode weapon) => weapon.shotAnimation.Play();

        public class InitialShotAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;

            public ShotAnimationComponent shotAnimation;
            public TankGroupComponent tankGroup;

            public WeaponCooldownComponent weaponCooldown;
        }

        public class ReadyShotAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public ShotAnimationComponent shotAnimation;

            public ShotAnimationReadyComponent shotAnimationReady;
            public TankGroupComponent tankGroup;
        }
    }
}