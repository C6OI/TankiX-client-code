using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineCommonSoundsSystem : ECSSystem {
        [OnEventComplete]
        public void PlayExplosionSound(MineExplosionEvent e, MineNode mine, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            if ((bool)mine.mineSounds.ExplosionSound) {
                mine.mineSounds.ExplosionSound.Play();
            }
        }

        [OnEventComplete]
        public void PlayDeactivationSound(RemoveEffectEvent e, MineNode mine, [JoinByTank] SingleNode<RemoteTankComponent> tank,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            mine.mineSounds.DeactivationSound.Play();
        }

        [OnEventFire]
        public void PrepareMineForDropSound(MineDropEvent evt, SingleNode<MineConfigComponent> mine, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            mine.Entity.AddComponent<MineReadyForDropSoundEffectComponent>();
        }

        [OnEventFire]
        public void PlayDropSound(NodeAddedEvent evt, [Combine] MineDropSoundNode mine, SingleNode<MapDustComponent> map) {
            MinePlacingTransformComponent minePlacingTransform = mine.minePlacingTransform;

            if (!minePlacingTransform.HasPlacingTransform) {
                return;
            }

            Transform transform = minePlacingTransform.PlacingData.transform;
            Vector2 textureCoord = minePlacingTransform.PlacingData.textureCoord;
            DustEffectBehaviour effectByTag = map.component.GetEffectByTag(transform, textureCoord);

            if (!(effectByTag == null)) {
                AudioSource audioSource;

                switch (effectByTag.surface) {
                    default:
                        return;

                    case DustEffectBehaviour.SurfaceType.Metal:
                    case DustEffectBehaviour.SurfaceType.Concrete:
                        audioSource = mine.mineSounds.DropNonGroundSound;
                        break;

                    case DustEffectBehaviour.SurfaceType.Soil:
                    case DustEffectBehaviour.SurfaceType.Sand:
                    case DustEffectBehaviour.SurfaceType.Grass:
                        audioSource = mine.mineSounds.DropGroundSound;
                        break;
                }

                audioSource.Play();
            }
        }

        public class MineNode : Node {
            public MineSoundsComponent mineSounds;
        }

        public class MineDropSoundNode : MineNode {
            public MinePlacingTransformComponent minePlacingTransform;

            public MineReadyForDropSoundEffectComponent mineReadyForDropSoundEffect;
        }
    }
}