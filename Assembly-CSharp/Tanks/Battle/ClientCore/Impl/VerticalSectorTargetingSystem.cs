using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class VerticalSectorTargetingSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, WeaponNode weapon, [JoinByTank] TankNode tank) {
            TargetingData targetingData = evt.TargetingData;
            VerticalSectorsTargetingComponent verticalSectorsTargeting = weapon.verticalSectorsTargeting;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            targetingData.Origin = muzzleLogicAccessor.GetBarrelOriginWorld();
            targetingData.Dir = muzzleLogicAccessor.GetFireDirectionWorld();
            targetingData.FullDistance = verticalSectorsTargeting.WorkDistance;
            targetingData.MaxAngle = !(verticalSectorsTargeting.VAngleUp > verticalSectorsTargeting.VAngleDown) ? verticalSectorsTargeting.VAngleDown : verticalSectorsTargeting.VAngleUp;
            LinkedList<TargetSector> instance = BattleCache.targetSectors.GetInstance();
            instance.Clear();
            CollectTargetSectorsEvent collectTargetSectorsEvent = BattleCache.collectTargetSectorsEvent.GetInstance().Init();
            collectTargetSectorsEvent.TargetSectors = instance;

            collectTargetSectorsEvent.TargetingCone = new TargetingCone {
                VAngleUp = verticalSectorsTargeting.VAngleUp,
                VAngleDown = verticalSectorsTargeting.VAngleDown,
                HAngle = verticalSectorsTargeting.HAngle,
                Distance = verticalSectorsTargeting.WorkDistance
            };

            ScheduleEvent(collectTargetSectorsEvent, weapon);
            CollectSectorDirectionsEvent collectSectorDirectionsEvent = BattleCache.collectSectorDirectionsEvent.GetInstance().Init();
            collectSectorDirectionsEvent.TargetSectors = instance;
            collectSectorDirectionsEvent.TargetingData = targetingData;
            ScheduleEvent(collectSectorDirectionsEvent, weapon);
            ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), weapon);
            ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), weapon);
        }

        public class TankNode : Node {
            public TankComponent tank;
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}