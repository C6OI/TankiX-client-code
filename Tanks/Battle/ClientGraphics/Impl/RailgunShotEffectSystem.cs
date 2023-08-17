using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunShotEffectSystem : ECSSystem {
        [OnEventFire]
        public void Build(NodeAddedEvent evt, [Combine] RailgunSoundEffectNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            GameObject gameObject = Object.Instantiate(weapon.railgunShotEffect.Asset);
            gameObject.transform.parent = weapon.weaponSoundRoot.gameObject.transform;
            gameObject.transform.localPosition = Vector3.zero;
            AudioSource component = gameObject.GetComponent<AudioSource>();
            weapon.railgunShotEffect.AudioSurce = component;
            weapon.Entity.AddComponent<RailgunShotEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayShotEffect(BaseRailgunChargingShotEvent evt, RailgunSoundEffectReadyNode weapon,
            [JoinByTank] TankActiveNode tank) => weapon.railgunShotEffect.AudioSurce.Play();

        [OnEventFire]
        public void StopSound(NodeRemoveEvent evt, TankActiveNode tank, [JoinByTank] RailgunSoundEffectReadyNode weapon) {
            AudioSource audioSurce = weapon.railgunShotEffect.AudioSurce;

            if (audioSurce.isPlaying) {
                audioSurce.Stop();
            }
        }

        public class RailgunSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public RailgunChargingWeaponComponent railgunChargingWeapon;

            public RailgunShotEffectComponent railgunShotEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class RailgunSoundEffectReadyNode : RailgunSoundEffectNode {
            public RailgunShotEffectReadyComponent railgunShotEffectReady;
        }

        public class TankActiveNode : Node {
            public TankActiveStateComponent tankActiveState;
        }
    }
}