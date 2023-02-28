using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ConicDirectionsCollectorSystem : AbstractDirectionsCollectorSystem {
        [OnEventFire]
        public void CollectDirections(CollectDirectionsEvent evt, TargetingNode conicTargeting) {
            TargetingData targetingData = evt.TargetingData;
            ConicTargetingComponent conicTargeting2 = conicTargeting.conicTargeting;
            CollectDirection(targetingData.Origin, targetingData.Dir, 0f, targetingData);
            float num = conicTargeting2.HalfConeAngle / conicTargeting2.HalfConeNumRays;
            Vector3 vector = new MuzzleLogicAccessor(conicTargeting.muzzlePoint, conicTargeting.weaponInstance).GetLeftDirectionWorld();

            for (int i = 0; i < conicTargeting2.NumSteps; i++) {
                CollectSectorDirections(targetingData.Origin, targetingData.Dir, vector, num, conicTargeting2.HalfConeNumRays, targetingData);
                CollectSectorDirections(targetingData.Origin, targetingData.Dir, vector, 0f - num, conicTargeting2.HalfConeNumRays, targetingData);
                vector = Quaternion.AngleAxis(180f / conicTargeting2.NumSteps, targetingData.Dir) * vector;
            }
        }

        public class TargetingNode : Node {
            public ConicTargetingComponent conicTargeting;

            public MuzzlePointComponent muzzlePoint;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}