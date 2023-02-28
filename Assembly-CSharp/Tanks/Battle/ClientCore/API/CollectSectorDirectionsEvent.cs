using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class CollectSectorDirectionsEvent : Event {
        public ICollection<TargetSector> TargetSectors { get; set; }

        public TargetingData TargetingData { get; set; }

        public CollectSectorDirectionsEvent Init() {
            TargetSectors = null;
            TargetingData = null;
            return this;
        }
    }
}