using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingTargetCollectorSystem : AbstractTargetCollectorSystem {
        [OnEventFire]
        public void CollectTargetsOnDirections(ShaftAimingCollectTargetsEvent evt,
            SingleNode<TargetCollectorComponent> targetCollectorNode,
            [JoinByTank] SingleNode<TankCollidersComponent> tankCollidersNode,
            [JoinAll]
            ICollection<SingleNode<TankPartIntersectedWithCameraStateComponent>> intersectedWithCameraTankPartList) {
            TargetingData targetingData = evt.TargetingData;

            CollectTargetsWithExclusion(targetingData,
                tankCollidersNode.component.VisualTriggerColliders,
                TargetValidator,
                GetLocalHitPointByVisual,
                LayerMasks.VISUAL_TARGETING,
                IsDamagerInsideTarget(intersectedWithCameraTankPartList));
        }

        static bool IsDamagerInsideTarget(
            ICollection<SingleNode<TankPartIntersectedWithCameraStateComponent>> intersectedWithCameraTankPartList) =>
            intersectedWithCameraTankPartList.Count > 1;

        static bool TargetValidator(Entity target, RaycastHit hitInfo, bool isInsideAttack) {
            if (!target.HasComponent<TankActiveStateComponent>()) {
                return false;
            }

            if (isInsideAttack) {
                return true;
            }

            return !CheckTargetCollision(hitInfo, LayerMasks.VISUAL_STATIC);
        }
    }
}