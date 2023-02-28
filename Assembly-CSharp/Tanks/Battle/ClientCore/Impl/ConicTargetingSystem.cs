using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ConicTargetingSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, TargetingNode conicTargeting) {
            TargetingData targetingData = evt.TargetingData;
            ConicTargetingComponent conicTargeting2 = conicTargeting.conicTargeting;
            MuzzleLogicAccessor muzzleLogicAccessor = new(conicTargeting.muzzlePoint, conicTargeting.weaponInstance);
            targetingData.Origin = muzzleLogicAccessor.GetWorldPositionShiftDirectionBarrel(conicTargeting2.FireOriginOffsetCoeff);
            targetingData.Dir = muzzleLogicAccessor.GetFireDirectionWorld();
            targetingData.FullDistance = conicTargeting2.WorkDistance;
            targetingData.MaxAngle = conicTargeting2.HalfConeAngle;
            ScheduleEvent(BattleCache.collectDirectionsEvent.GetInstance().Init(targetingData), conicTargeting);
            ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), conicTargeting);
            ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), conicTargeting);
        }

        public class TargetingNode : Node {
            public ConicTargetingComponent conicTargeting;

            public MuzzlePointComponent muzzlePoint;

            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}