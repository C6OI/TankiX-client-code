using System.Collections.Generic;

namespace Tanks.Battle.ClientCore.API {
    public class HitTargetAdapter {
        public static List<HitTarget> Adapt(List<TargetData> targetsData) {
            List<HitTarget> list = new();

            for (int i = 0; i < targetsData.Count; i++) {
                TargetData targetData = targetsData[i];
                list.Add(Adapt(targetData));
            }

            return list;
        }

        public static HitTarget Adapt(TargetData targetData) {
            HitTarget hitTarget = new();
            hitTarget.Entity = targetData.TargetEntity;
            hitTarget.IncarnationEntity = targetData.TargetIncorantionEntity;
            hitTarget.LocalHitPoint = targetData.LocalHitPoint;
            hitTarget.TargetPosition = targetData.TargetPosition;
            hitTarget.HitDirection = targetData.HitDirection;
            hitTarget.HitDistance = targetData.HitDistance;
            return hitTarget;
        }
    }
}