using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HammerHitSoundEffectSystem : BaseHitExplosionSoundSystem {
        [OnEventFire]
        public void CreateHitSoundEffect(HitEvent evt, HammerHitSoundEffectNode weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            List<HitTarget> targets = evt.Targets;
            HammerHitSoundEffectComponent hammerHitSoundEffect = weapon.hammerHitSoundEffect;

            if (targets != null) {
                int count = targets.Count;

                if (count > 0) {
                    List<HitTarget> differentTargetsByHit = hammerHitSoundEffect.DifferentTargetsByHit;
                    differentTargetsByHit.Clear();

                    for (int i = 0; i < count; i++) {
                        HitTarget item = targets[i];

                        if (!differentTargetsByHit.Contains(item)) {
                            differentTargetsByHit.Add(item);
                        }
                    }

                    Vector3 zero = Vector3.zero;
                    int count2 = differentTargetsByHit.Count;

                    for (int j = 0; j < count2; j++) {
                        zero += differentTargetsByHit[j].TargetPosition;
                    }

                    zero /= count2;
                    GameObject targetHitSoundAsset = hammerHitSoundEffect.TargetHitSoundAsset;
                    float targetHitSoundDuration = hammerHitSoundEffect.TargetHitSoundDuration;
                    CreateHitExplosionSoundEffect(zero, targetHitSoundAsset, targetHitSoundDuration);
                    return;
                }
            }

            if (evt.StaticHit != null) {
                GameObject staticHitSoundAsset = hammerHitSoundEffect.StaticHitSoundAsset;
                float staticHitSoundDuration = hammerHitSoundEffect.StaticHitSoundDuration;
                CreateHitExplosionSoundEffect(evt.StaticHit.Position, staticHitSoundAsset, staticHitSoundDuration);
            }
        }

        public class HammerHitSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;
            public HammerHitSoundEffectComponent hammerHitSoundEffect;
        }
    }
}