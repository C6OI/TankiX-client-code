using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class VerticalSectorTargetingSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void PrepareTargeting(TargetingEvent evt, TargetingNode verticalTargeting) {
            TargetingData targetingData = evt.TargetingData;
            VerticalSectorsTargetingComponent verticalSectorsTargeting = verticalTargeting.verticalSectorsTargeting;
            MuzzleLogicAccessor muzzleLogicAccessor = new(verticalTargeting.muzzlePoint, verticalTargeting.weaponInstance);
            targetingData.Origin = muzzleLogicAccessor.GetBarrelOriginWorld();
            targetingData.Dir = muzzleLogicAccessor.GetFireDirectionWorld();
            targetingData.FullDistance = verticalSectorsTargeting.WorkDistance;

            targetingData.MaxAngle = !(verticalSectorsTargeting.VAngleUp > verticalSectorsTargeting.VAngleDown)
                                         ? verticalSectorsTargeting.VAngleDown : verticalSectorsTargeting.VAngleUp;

            LinkedList<TargetSector> targetSectors = new();
            CollectTargetSectorsEvent collectTargetSectorsEvent = new();
            collectTargetSectorsEvent.TargetSectors = targetSectors;

            collectTargetSectorsEvent.TargetingCone = new TargetingCone {
                VAngleUp = verticalSectorsTargeting.VAngleUp,
                VAngleDown = verticalSectorsTargeting.VAngleDown,
                HAngle = verticalSectorsTargeting.HAngle,
                Distance = verticalSectorsTargeting.WorkDistance
            };

            ScheduleEvent(collectTargetSectorsEvent, verticalTargeting);
            CollectSectorDirectionsEvent collectSectorDirectionsEvent = new();
            collectSectorDirectionsEvent.TargetSectors = targetSectors;
            collectSectorDirectionsEvent.TargetingData = targetingData;
            CollectSectorDirectionsEvent eventInstance = collectSectorDirectionsEvent;
            ScheduleEvent(eventInstance, verticalTargeting);
            ScheduleEvent(BattleCache.collectTargetsEvent.GetInstance().Init(targetingData), verticalTargeting);
            ScheduleEvent(BattleCache.targetEvaluateEvent.GetInstance().Init(targetingData), verticalTargeting);
        }

        public class TargetingNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}