using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    public class BattleFlowInstancesCache : AbstratFlowInstancesCache {
        public readonly Cache<CollectDirectionsEvent> collectDirectionsEvent;

        public readonly Cache<CollectSectorDirectionsEvent> collectSectorDirectionsEvent;

        public readonly Cache<CollectTargetSectorsEvent> collectTargetSectorsEvent;

        public readonly Cache<CollectTargetsEvent> collectTargetsEvent;
        public readonly Cache<DirectionData> directionData;

        public readonly Cache<TargetData> targetData;

        public readonly Cache<TargetingEvaluateEvent> targetEvaluateEvent;

        public readonly Cache<TargetingData> targetingData;

        public readonly Cache<TargetingEvent> targetingEvent;

        public readonly Cache<LinkedList<TargetSector>> targetSectors;

        public readonly Cache<UpdateBulletEvent> updateBulletEvent;

        public BattleFlowInstancesCache() {
            targetingData = Register<TargetingData>();
            directionData = Register<DirectionData>();
            targetData = Register<TargetData>();
            targetingEvent = Register<TargetingEvent>();
            targetEvaluateEvent = Register<TargetingEvaluateEvent>();
            collectDirectionsEvent = Register<CollectDirectionsEvent>();
            collectTargetsEvent = Register<CollectTargetsEvent>();
            updateBulletEvent = Register<UpdateBulletEvent>();
            targetSectors = Register<LinkedList<TargetSector>>();
            collectTargetSectorsEvent = Register<CollectTargetSectorsEvent>();
            collectSectorDirectionsEvent = Register<CollectSectorDirectionsEvent>();
        }
    }
}