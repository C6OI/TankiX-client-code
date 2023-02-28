using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ForceFieldEffectSystem : ECSSystem {
        [OnEventFire]
        public void FindLocation(NodeAddedEvent e, ForceFieldEffectNode effect, [Context] [JoinByTank] SelfWeaponNode weaponNode) {
            Transform transform = weaponNode.weaponInstance.WeaponInstance.transform;
            ForceFieldTranformComponent transformComponent = ForceFieldTransformUtil.GetTransformComponent(transform);
            effect.Entity.AddComponent(transformComponent);
        }

        [OnEventFire]
        public void CreateVisualEffect(NodeAddedEvent e, SingleNode<PreloadedModuleEffectsComponent> mapEffect, [Combine] ForceFieldEffectTransformNode effect) {
            GameObject gameObject = mapEffect.component.PreloadedEffects["forcefield"];

            if ((bool)gameObject) {
                GameObject gameObject2 = Object.Instantiate(gameObject, null);
                gameObject2.SetActive(true);
                gameObject2.transform.SetPositionSafe(effect.forceFieldTranform.Movement.Position);
                gameObject2.transform.SetRotationSafe(effect.forceFieldTranform.Movement.Orientation);
                gameObject2.GetComponent<EntityBehaviour>().BuildEntity(effect.Entity);
                gameObject2.SetActive(true);
                ForceFieldEffect component = gameObject2.GetComponent<ForceFieldEffect>();
                component.Show();
                component.SetLayer(Layers.VISUAL_STATIC);
                effect.Entity.AddComponent(new EffectInstanceComponent(gameObject2));
                gameObject2.AddComponent<Rigidbody>().isKinematic = true;
                ForcefieldTargetBehaviour forcefieldTargetBehaviour = gameObject2.AddComponent<ForcefieldTargetBehaviour>();
                forcefieldTargetBehaviour.OwnerTeamCanShootThrough = effect.forceFieldEffect.OwnerTeamCanShootThrough;
                forcefieldTargetBehaviour.Init(effect.Entity);
            }
        }

        [OnEventFire]
        public void InitForceFieldForSpectator(NodeAddedEvent e, SpectatorBattleUserNode spectator, [Context] [Combine] TeamForceFieldInstanceNode forceField,
            [Context] [JoinByTeam] TeamColorNode teamColor) {
            if (teamColor.teamColor.TeamColor == TeamColor.RED) {
                forceField.effectInstance.GameObject.GetComponent<ForceFieldEffect>().SwitchToEnemyView();
            }
        }

        [OnEventFire]
        public void InitForceFieldColor(NodeAddedEvent e, SelfBattleUserNode selfUser, [Combine] TeamForceFieldInstanceNode forceField) {
            if (!selfUser.Entity.IsSameGroup<TeamGroupComponent>(forceField.Entity)) {
                forceField.effectInstance.GameObject.GetComponent<ForceFieldEffect>().SwitchToEnemyView();
            }
        }

        [OnEventFire]
        public void InitForceFieldColorForDm(NodeAddedEvent e, SelfDmBattleUserNode selfUser, [JoinByUser] SingleNode<TankComponent> tank,
            [Combine] ForceFieldEffectInstanceNode forceField) {
            if (!tank.Entity.IsSameGroup<TankGroupComponent>(forceField.Entity)) {
                forceField.effectInstance.GameObject.GetComponent<ForceFieldEffect>().SwitchToEnemyView();
            }
        }

        [OnEventFire]
        public void HideVisualEffect(NodeRemoveEvent e, ForceFieldEffectInstanceNode fieldEffectNode) {
            fieldEffectNode.effectInstance.GameObject.GetComponent<ForceFieldEffect>().Hide();
        }

        [OnEventFire]
        public void DrawWavesOnHit(HitEvent e, NotStreamWeaponNode node, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects) {
            if (e.StaticHit != null) {
                DrawWavesOnHit(effects, e.StaticHit.Position, true);
            }
        }

        [OnEventFire]
        public void DrawWavesOnHit(BulletStaticHitEvent e, Node node, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects) {
            DrawWavesOnHit(effects, e.Position, true);
        }

        [OnEventComplete]
        public void DrawWavesOnHit(UpdateEvent evt, SingleNode<StreamHitComponent> weaponNode, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects) {
            if (weaponNode.component.StaticHit != null) {
                DrawWavesOnHit(effects, weaponNode.component.StaticHit.Position, false);
            }
        }

        [OnEventComplete]
        public void DrawWavesOnHitFromRicochet(RicochetBulletBounceEvent e, RicochetBulletNode ricochetBulletNode, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects) {
            Vector3 worldSpaceBouncePosition = e.WorldSpaceBouncePosition;
            DrawWavesOnHit(effects, worldSpaceBouncePosition, true);
        }

        void DrawWavesOnHit(ICollection<ForceFieldEffectInstanceNode> effects, Vector3 hitPosition, bool playSound) {
            foreach (ForceFieldEffectInstanceNode effect in effects) {
                ForceFieldEffect component = effect.effectInstance.GameObject.GetComponent<ForceFieldEffect>();
                MeshCollider outerMeshCollider = component.outerMeshCollider;

                if (Vector3.Distance(hitPosition, outerMeshCollider.ClosestPointOnBounds(hitPosition)) < 0.1f) {
                    component.DrawWave(hitPosition, playSound);
                }
            }
        }

        public class ForceFieldEffectNode : Node {
            public ForceFieldEffectComponent forceFieldEffect;

            public TankGroupComponent tankGroup;
        }

        public class ForceFieldEffectTransformNode : ForceFieldEffectNode {
            public ForceFieldTranformComponent forceFieldTranform;
        }

        public class ForceFieldEffectInstanceNode : ForceFieldEffectTransformNode {
            public EffectInstanceComponent effectInstance;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;
        }

        public class SelfWeaponNode : WeaponNode {
            public SelfComponent self;
        }

        public class RicochetBulletNode : Node {
            public BulletComponent bullet;

            public RicochetBulletComponent ricochetBullet;
        }

        [Not(typeof(StreamWeaponComponent))]
        [Not(typeof(StreamHitConfigComponent))]
        public class NotStreamWeaponNode : Node {
            public WeaponComponent weapon;
        }

        public class SelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TeamGroupComponent))]
        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class SelfDmBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SpectatorBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class TeamColorNode : Node {
            public TeamColorComponent teamColor;
            public TeamGroupComponent teamGroup;
        }

        public class TeamForceFieldInstanceNode : ForceFieldEffectInstanceNode {
            public TeamGroupComponent teamGroup;
        }
    }
}