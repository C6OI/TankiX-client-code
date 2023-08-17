using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public abstract class AbstractBulletSystem : ECSSystem {
        protected void InitBullet(BulletComponent bullet, Vector3 position, Vector3 direction, float radius,
            Rigidbody tankRigidbody) {
            position += direction * radius;

            position += direction *
                        Vector3.Dot(direction, tankRigidbody.velocity.normalized) *
                        tankRigidbody.velocity.magnitude *
                        Time.smoothDeltaTime;

            bullet.LastUpdateTime = Time.time;
            bullet.Position = position;
            bullet.Direction = direction;
        }

        protected void MoveBullet(BulletComponent bullet) {
            float num = Time.time - bullet.LastUpdateTime;
            float num2 = bullet.Speed * num;
            bullet.Position += num2 * bullet.Direction;
            bullet.Distance += num2;
            bullet.LastUpdateTime = Time.time;
        }

        protected bool DestroyOnAnyTargetHit(Entity bulletEntity, BulletComponent bullet, BulletConfigComponent config,
            TargetingData targeting) {
            List<DirectionData> directions = targeting.Directions;

            foreach (DirectionData item in directions) {
                if (item.Targets.Count > 0) {
                    TargetData targetData = item.Targets.First();
                    SetPositionNearHitPoint(bullet, targetData.HitPoint);
                    SendBulletHitEvent(bulletEntity, bullet);
                    DestroyBullet(bulletEntity);
                    return true;
                }
            }

            return false;
        }

        protected void SetPositionNearHitPoint(BulletComponent bullet, Vector3 hitPoint) {
            Vector3 vector = hitPoint - bullet.Position;
            float num = Vector3.Dot(bullet.Direction, vector.normalized);
            bullet.Position += bullet.Direction * (vector.magnitude * num - bullet.Radius);
        }

        protected void SendBulletHitEvent(Entity bulletEntity, BulletComponent bullet) =>
            ScheduleEvent(new BulletHitEvent(bullet.Position), bulletEntity);

        protected void DestroyBullet(Entity bulletEntity) {
            bulletEntity.RemoveComponent<BulletComponent>();
            DeleteEntity(bulletEntity);
        }

        protected void PrepareTargetData(TargetData targetData, BulletComponent bulletComponent) {
            targetData.HitDistance += bulletComponent.Distance;
            targetData.HitDirection = bulletComponent.Direction;
        }
    }
}