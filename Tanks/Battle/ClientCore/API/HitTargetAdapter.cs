using System.Collections.Generic;

namespace Tanks.Battle.ClientCore.API {
    public class HitTargetAdapter {
        public static List<HitTarget> Adapt(List<TargetData> targetsData) {
            List<HitTarget> list = new();

            foreach (TargetData targetsDatum in targetsData) {
                list.Add(Adapt(targetsDatum));
            }

            return list;
        }

        public static HitTarget Adapt(TargetData targetData) {
            HitTarget hitTarget = new();
            hitTarget.Entity = targetData.Entity;
            hitTarget.LocalHitPoint = targetData.LocalHitPoint;
            hitTarget.TargetPosition = targetData.TargetPosition;
            hitTarget.HitDirection = targetData.HitDirection;
            hitTarget.HitDistance = targetData.HitDistance;
            return hitTarget;
        }
    }
}