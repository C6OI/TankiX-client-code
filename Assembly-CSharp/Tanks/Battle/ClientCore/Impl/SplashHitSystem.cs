using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SplashHitSystem : ECSSystem {
        const float FORWARD_SPLASH_OFFSET = 0.25f;

        const float STATIC_HIT_SPLASH_CENTER_OFFSET = 0.01f;

        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventComplete]
        public void PrepareTargetsByUnblockedWeapon(ShotPrepareEvent evt, UnblockedWeaponNode weaponNode) {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            ScheduleEvent(new TargetingEvent(targetingData), weaponNode);
            ScheduleEvent(new SendShotToServerEvent(targetingData), weaponNode);
            ScheduleEvent(new SendHitToServerIfNeedEvent(targetingData), weaponNode);
        }

        [OnEventComplete]
        public void PrepareSplashTargetsWhenBlockedWeapon(ShotPrepareEvent evt, BlockedWeaponNode weapon) {
            WeaponBlockedComponent weaponBlocked = weapon.weaponBlocked;
            StaticHit staticHit = new();
            Vector3 blockPoint = weaponBlocked.BlockPoint;
            staticHit.Position = blockPoint;
            staticHit.Normal = weaponBlocked.BlockNormal;
            List<HitTarget> directTargets = new();
            SplashHitData splashHit = SplashHitData.CreateSplashHitData(directTargets, staticHit, weapon.Entity);
            ScheduleEvent(new CollectSplashTargetsEvent(splashHit), weapon);
        }

        [OnEventFire]
        public void PrepareHitByUnblockedWeapon(SendHitToServerIfNeedEvent evt, UnblockedWeaponNode weapon) {
            TargetingData targetingData = evt.TargetingData;
            DirectionData bestDirection = targetingData.BestDirection;

            if (bestDirection.HasAnyHit()) {
                List<HitTarget> directTargets = HitTargetAdapter.Adapt(bestDirection.Targets);
                StaticHit staticHit = bestDirection.StaticHit;
                SplashHitData splashHit = SplashHitData.CreateSplashHitData(directTargets, staticHit, weapon.Entity);
                ScheduleEvent(new CollectSplashTargetsEvent(splashHit), weapon);
            }
        }

        [OnEventFire]
        public void PrepareHit(CollectSplashTargetsEvent evt, SplashWeaponNode weapon) {
            SplashHitData splashHit = evt.SplashHit;
            List<HitTarget> directTargets = splashHit.DirectTargets;
            int count = directTargets.Count;

            if (count > 0) {
                HitTarget hitTarget = directTargets.First();
                Vector3 localHitPoint = hitTarget.LocalHitPoint;
                Entity entity = hitTarget.Entity;
                ScheduleEvent(new CalculateSplashCenterByDirectTargetEvent(splashHit, localHitPoint), entity);
            } else {
                ScheduleEvent(new CalculateSplashCenterByStaticHitEvent(splashHit), weapon.Entity);
            }
        }

        [OnEventComplete]
        public void SendHitEvent(CollectSplashTargetsEvent evt, SplashWeaponNode weapon) {
            SplashHitData splashHit = evt.SplashHit;
            SelfSplashHitEvent eventInstance = new(splashHit.DirectTargets, splashHit.StaticHit, splashHit.SplashTargets);
            ScheduleEvent(eventInstance, weapon);
        }

        [OnEventFire]
        public void CalculateSplashCenterByStatic(CalculateSplashCenterByStaticHitEvent evt, SplashWeaponNode weapon) {
            SplashHitData splashHit = evt.SplashHit;
            StaticHit staticHit = splashHit.StaticHit;
            splashHit.SplashCenter = staticHit.Position + staticHit.Normal * 0.01f;
        }

        [OnEventFire]
        public void CalculateSplashCenterByTarget(CalculateSplashCenterByDirectTargetEvent evt, TankNode tank) {
            SplashHitData splashHit = evt.SplashHit;
            splashHit.SplashCenter = MathUtil.LocalPositionToWorldPosition(evt.DirectTargetLocalHitPoint, tank.rigidbody.Rigidbody.gameObject);
            splashHit.ExcludedEntityForSplashHit = new HashSet<Entity> { tank.Entity };
            splashHit.ExclusionGameObjectForSplashRaycast.AddRange(tank.tankColliders.TargetingColliders);
        }

        [OnEventComplete]
        public void FinalizeCalculationSplashCenter(CalculateSplashCenterEvent evt, Node node) {
            SplashHitData splashHit = evt.SplashHit;

            if (splashHit.SplashCenterInitialized) {
                ScheduleEvent(new CalculateSplashTargetsWithCenterEvent(splashHit), splashHit.WeaponHitEntity);
            }
        }

        [OnEventFire]
        public void CalculateSplashTargetsList(CalculateSplashTargetsWithCenterEvent evt, SplashWeaponNode weapon, [JoinByBattle] ICollection<ActiveTankNode> activeTanks) {
            SplashHitData splashHit = evt.SplashHit;
            HashSet<Entity> excludedEntityForSplashHit = splashHit.ExcludedEntityForSplashHit;

            foreach (ActiveTankNode activeTank in activeTanks) {
                if (excludedEntityForSplashHit == null || !excludedEntityForSplashHit.Contains(activeTank.Entity)) {
                    ValidateSplashHitPointsEvent eventInstance = new(splashHit, splashHit.ExclusionGameObjectForSplashRaycast);
                    NewEvent(eventInstance).Attach(weapon).Attach(activeTank).Schedule();
                }
            }
        }

        [OnEventFire]
        public void ValidateSplashHitTargetByWeaponPoint(ValidateSplashHitPointsEvent evt, SplashWeaponNode weaponHit, ActiveTankNode targetTank,
            [JoinByTank] SingleNode<TankIncarnationComponent> targetIncarnation, [JoinByTank] WeaponBoundsNode weaponBoundsTarget) {
            TankCollidersComponent tankColliders = targetTank.tankColliders;
            BoxCollider boundsCollider = tankColliders.BoundsCollider;
            float num = weaponBoundsTarget.weaponBounds.WeaponBounds.size.y * 0.5f;
            Vector3 position = targetTank.mountPoint.MountPoint.position;
            Vector3 item = position + boundsCollider.transform.up * num;
            float radiusOfMinSplashDamage = weaponHit.splashWeapon.RadiusOfMinSplashDamage;
            SplashHitData splashHit = evt.SplashHit;
            Vector3 splashCenter = splashHit.SplashCenter;
            List<HitTarget> splashTargets = splashHit.SplashTargets;
            List<GameObject> exclusionGameObjectForSplashRaycast = splashHit.ExclusionGameObjectForSplashRaycast;
            List<GameObject> targetingColliders = tankColliders.TargetingColliders;
            Vector3 position2 = targetTank.rigidbody.Rigidbody.position;
            Vector3 vector = position2 - splashCenter;
            float num2 = boundsCollider.size.z * 0.25f;
            Vector3 vector2 = boundsCollider.transform.forward * num2;
            Vector3 center = boundsCollider.bounds.center;
            Vector3 item2 = center + vector2;
            Vector3 item3 = center - vector2;
            List<Vector3> list = new();
            list.Add(item);
            list.Add(center);
            list.Add(item3);
            list.Add(item2);
            list.Add(position);
            List<Vector3> list2 = list;

            foreach (Vector3 item5 in list2) {
                if (IsValidSplashPoint(targetTank, item5, splashCenter, evt, radiusOfMinSplashDamage)) {
                    HitTarget hitTarget = new();
                    hitTarget.Entity = targetTank.Entity;
                    hitTarget.IncarnationEntity = targetIncarnation.Entity;
                    hitTarget.LocalHitPoint = Vector3.zero;
                    hitTarget.TargetPosition = position2;
                    hitTarget.HitDirection = vector.normalized;
                    hitTarget.HitDistance = vector.magnitude;
                    HitTarget item4 = hitTarget;
                    splashTargets.Add(item4);
                    exclusionGameObjectForSplashRaycast.AddRange(targetingColliders);
                    break;
                }
            }
        }

        bool IsValidSplashPoint(ActiveTankNode activeTank, Vector3 splashPositionForValidation, Vector3 splashCenter, ValidateSplashHitPointsEvent e, float radius) {
            if (!PhysicsUtil.ValidateVector3(splashCenter)) {
                return false;
            }

            if (!PhysicsUtil.ValidateVector3(splashPositionForValidation)) {
                return false;
            }

            using (new RaycastExclude(e.excludeObjects)) {
                if ((splashPositionForValidation - splashCenter).magnitude > radius) {
                    return false;
                }

                return !IsPointOccluded(activeTank, splashCenter, splashPositionForValidation);
            }
        }

        bool IsPointOccluded(ActiveTankNode activeTank, Vector3 splashCenter, Vector3 tankPosition) {
            Vector3 vector = tankPosition - splashCenter;
            Vector3 normalized = vector.normalized;
            RaycastHit hitInfo;

            if (!Physics.Raycast(splashCenter, normalized, out hitInfo, vector.magnitude, LayerMasks.GUN_TARGETING_WITHOUT_DEAD_UNITS)) {
                return false;
            }

            GameObject gameObject = hitInfo.transform.gameObject;
            TargetBehaviour componentInParent = gameObject.GetComponentInParent<TargetBehaviour>();

            if (!IsValidTarget(componentInParent)) {
                return true;
            }

            return componentInParent.TargetEntity != activeTank.Entity;
        }

        bool IsValidTarget(TargetBehaviour targetBehaviour) => targetBehaviour != null && targetBehaviour.TargetEntity.HasComponent<TankActiveStateComponent>();

        public class WeaponBoundsNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponBoundsComponent weaponBounds;
        }

        public class SplashWeaponNode : Node {
            public BattleGroupComponent battleGroup;
            public SplashWeaponComponent splashWeapon;
        }

        public class UnblockedWeaponNode : SplashWeaponNode {
            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class BlockedWeaponNode : SplashWeaponNode {
            public WeaponBlockedComponent weaponBlocked;
        }

        public class TankNode : Node {
            public BattleGroupComponent battleGroup;

            public MountPointComponent mountPoint;

            public RigidbodyComponent rigidbody;
            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class ActiveTankNode : TankNode {
            public TankActiveStateComponent tankActiveState;
        }
    }
}