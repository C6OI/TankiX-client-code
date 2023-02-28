using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DiscreteWeaponShotEffectSystem : ECSSystem {
        [OnEventFire]
        public void Build(NodeAddedEvent evt, [Combine] DiscreteWeaponSoundEffectNode node, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            DiscreteWeaponShotEffectComponent discreteWeaponShotEffect = node.discreteWeaponShotEffect;
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            discreteWeaponShotEffect.AudioSources = new AudioSource[muzzlePoint.Points.Length];

            for (int i = 0; i < muzzlePoint.Points.Length; i++) {
                GameObject gameObject = Object.Instantiate(discreteWeaponShotEffect.Asset);
                discreteWeaponShotEffect.AudioSources[i] = gameObject.GetComponent<AudioSource>();
                gameObject.transform.parent = node.weaponSoundRoot.gameObject.transform;
                gameObject.transform.SetPositionSafe(muzzlePoint.Points[i].position);
            }

            if (!node.Entity.HasComponent<DiscreteWeaponShotEffectReadyComponent>()) {
                node.Entity.AddComponent<DiscreteWeaponShotEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void PlayShotEffect(BaseShotEvent evt, DiscreteWeaponSoundEffectReadyNode node) {
            DiscreteWeaponShotEffectComponent discreteWeaponShotEffect = node.discreteWeaponShotEffect;
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            discreteWeaponShotEffect.AudioSources[muzzlePoint.CurrentIndex].Stop();
            discreteWeaponShotEffect.AudioSources[muzzlePoint.CurrentIndex].Play();
        }

        public class DiscreteWeaponSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;
            public DiscreteWeaponComponent discreteWeapon;

            public DiscreteWeaponShotEffectComponent discreteWeaponShotEffect;

            public MuzzlePointComponent muzzlePoint;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class DiscreteWeaponSoundEffectReadyNode : DiscreteWeaponSoundEffectNode {
            public DiscreteWeaponShotEffectReadyComponent discreteWeaponShotEffectReady;
        }
    }
}