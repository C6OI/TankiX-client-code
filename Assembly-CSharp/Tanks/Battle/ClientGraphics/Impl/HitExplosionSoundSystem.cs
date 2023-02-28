using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HitExplosionSoundSystem : BaseHitExplosionSoundSystem {
        [OnEventFire]
        public void CreateHitSoundEffect(HitEvent evt, HitExplosionSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            HitExplosionSoundComponent hitExplosionSound = weapon.hitExplosionSound;
            GameObject soundPrefab = hitExplosionSound.SoundPrefab;
            float duration = hitExplosionSound.Duration;

            if (evt.Targets != null) {
                foreach (HitTarget target in evt.Targets) {
                    CreateHitExplosionSoundEffect(target.TargetPosition, soundPrefab, duration);
                }
            }

            if (evt.StaticHit != null) {
                CreateHitExplosionSoundEffect(evt.StaticHit.Position, soundPrefab, duration);
            }
        }

        [OnEventFire]
        public void Explosion(BulletHitEvent e, Node node, [JoinByTank] HitExplosionSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            HitExplosionSoundComponent hitExplosionSound = weapon.hitExplosionSound;
            GameObject soundPrefab = hitExplosionSound.SoundPrefab;
            float duration = hitExplosionSound.Duration;
            CreateHitExplosionSoundEffect(e.Position, soundPrefab, duration);
        }

        public class HitExplosionSoundNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public HitExplosionSoundComponent hitExplosionSound;

            public TankGroupComponent tankGroup;
        }
    }
}