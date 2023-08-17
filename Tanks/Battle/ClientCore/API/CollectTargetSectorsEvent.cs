using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class CollectTargetSectorsEvent : Event {
        public TargetingCone TargetingCone { get; set; }

        public ICollection<TargetSector> TargetSectors { get; set; }

        public float HAllowableAngleAcatter { get; set; }

        public float VAllowableAngleAcatter { get; set; }
    }
}