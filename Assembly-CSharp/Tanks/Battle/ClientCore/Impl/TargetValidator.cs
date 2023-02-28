using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TargetValidator {
        public static float STRIKE_EPSILON = 0.05f;

        protected int hitCount;

        protected Entity ownerEntity;

        public TargetValidator(Entity ownerEntity) {
            LayerMask = LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS;
            this.ownerEntity = ownerEntity;
            ExcludeSelf = true;
            ExcludeDead = true;
        }

        public int LayerMask { get; set; }

        public bool ExcludeSelf { get; set; }

        public bool ExcludeDead { get; set; }

        public virtual void Begin() {
            hitCount = 0;
        }

        public virtual bool BreakOnStaticHit() => true;

        public virtual bool CanSkip(Entity targetEntity) {
            if (ExcludeSelf && targetEntity.Equals(ownerEntity)) {
                return true;
            }

            return false;
        }

        public virtual bool AcceptAsTarget(Entity targetEntity) {
            if (ExcludeDead && !targetEntity.HasComponent<TankActiveStateComponent>()) {
                return false;
            }

            hitCount++;
            return true;
        }

        public virtual void FillTargetData(TargetData targetData, RaycastHit hitInfo, GameObject hitRootGo, Ray ray, float fullDistance) {
            targetData.HitPoint = hitInfo.point;
            targetData.LocalHitPoint = MathUtil.WorldPositionToLocalPosition(PhysicsUtil.GetPulledHitPoint(hitInfo), hitRootGo);
            targetData.TargetPosition = hitRootGo.transform.position;
            targetData.HitDistance = fullDistance;
            targetData.HitDirection = ray.direction;
            targetData.PriorityWeakeningCount = hitCount - 1;
        }

        public virtual bool BreakOnTargetHit(Entity target) => true;

        public virtual Ray Continue(Ray direction, float distance) {
            direction.origin += direction.direction * (distance + STRIKE_EPSILON);
            return direction;
        }

        public virtual Ray ContinueOnStaticHit(Ray direction, Vector3 hitNormal, float distance) {
            direction.origin += direction.direction * (distance + STRIKE_EPSILON);
            return direction;
        }

        public virtual Ray ContinueOnTargetHit(Ray direction, Vector3 hitNormal, float distance) {
            direction.origin += direction.direction * (distance + STRIKE_EPSILON);
            return direction;
        }
    }
}