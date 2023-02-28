using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class CommonMineSystem : ECSSystem {
        static readonly float TANK_MINE_RAYCAST_DISTANCE = 10000f;

        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, [Combine] AnyMineNode mine, [JoinByUser] Optional<MountedGraffityItemNode> graffity,
            [JoinByUser] Optional<SingleNode<UserAvatarComponent>> avatar, SingleNode<MapInstanceComponent> map, SingleNode<PreloadedModuleEffectsComponent> mapEffect) {
            string text = mine.preloadingMineKey.Key;

            if (avatar.IsPresent()) {
                text = TryCrutchRemapByAvatar(text, avatar.Get().component.Id);
            }

            if (graffity.IsPresent()) {
                text = TryCrutchRemapByGraffiti(text, graffity.Get());
            }

            GameObject gameObject = mapEffect.component.PreloadedEffects[text];

            if ((bool)gameObject) {
                GameObject gameObject2 = Object.Instantiate(gameObject, null);
                gameObject2.SetActive(true);
                mine.Entity.AddComponent(new EffectInstanceComponent(gameObject2));
                gameObject2.GetComponent<EntityBehaviour>().BuildEntity(mine.Entity);
            }
        }

        string TryCrutchRemapByGraffiti(string existingKey, MountedGraffityItemNode graffiti) {
            if (existingKey == "spider" && graffiti.assetReference.Reference.AssetGuid == "7997b10cf40900d4f968f6d04619723d") {
                return "hellSpider";
            }

            return existingKey;
        }

        string TryCrutchRemapByAvatar(string existingKey, string avatarId) {
            if (existingKey == "spider" && avatarId == "457e8f5f-953a-424c-bd97-67d9e116ab7a") {
                return "spiderHolo";
            }

            if (existingKey == "mine" && avatarId == "457e8f5f-953a-424c-bd97-67d9e116ab7a") {
                return "mineHolo";
            }

            return existingKey;
        }

        [OnEventFire]
        public void TryExplosion(MineTryExplosionEvent evt, AnyActiveMineInstantiatedNode mine, [JoinByTank] SingleNode<SelfTankComponent> tank) {
            MineUtil.ExecuteSplashExplosion(mine.Entity, tank.Entity, mine.effectInstance.GameObject.transform.position);
        }

        [OnEventFire]
        public void InitMinePlacingTransform(InitMinePlacingTransformEvent e, SingleNode<MineConfigComponent> mine, SingleNode<MapInstanceComponent> map) {
            if (!mine.Entity.HasComponent<MinePlacingTransformComponent>()) {
                MinePlacingTransformComponent minePlacingTransformComponent = new();
                RaycastHit hitInfo;

                if (Physics.Raycast(e.Position + Vector3.up * 3f, Vector3.down, out hitInfo, TANK_MINE_RAYCAST_DISTANCE, LayerMasks.STATIC)) {
                    minePlacingTransformComponent.PlacingData = hitInfo;
                    minePlacingTransformComponent.HasPlacingTransform = true;
                } else {
                    minePlacingTransformComponent.HasPlacingTransform = false;
                }

                mine.Entity.AddComponent(minePlacingTransformComponent);
            }
        }

        public class AnyMineNode : Node {
            public MineConfigComponent mineConfig;

            public PreloadingMineKeyComponent preloadingMineKey;
            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
        }

        public class AnyActiveMineInstantiatedNode : AnyMineNode {
            public EffectActiveComponent effectActive;

            public EffectInstanceComponent effectInstance;
        }

        public class MountedGraffityItemNode : Node {
            public AssetReferenceComponent assetReference;
            public GraffitiBattleItemComponent graffitiBattleItem;
        }
    }
}