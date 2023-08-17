using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingStraightTargetingSystem : ECSSystem {
        [OnEventFire]
        public void FillTargetingData(ShaftAimingStraightTargetingEvent evt, ShaftAimingStraightTargetingNode weapon) {
            TargetingData targetingData = evt.TargetingData;
            VerticalSectorsTargetingComponent verticalSectorsTargeting = weapon.verticalSectorsTargeting;
            targetingData.Origin = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();
            targetingData.Dir = evt.WorkingDirection;
            targetingData.FullDistance = verticalSectorsTargeting.WorkDistance;
            ScheduleEvent(new ShaftAimingCollectDirectionEvent(targetingData), weapon);
            ScheduleEvent(new ShaftAimingCollectTargetsEvent(targetingData), weapon);
        }

        public class ShaftAimingStraightTargetingNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}