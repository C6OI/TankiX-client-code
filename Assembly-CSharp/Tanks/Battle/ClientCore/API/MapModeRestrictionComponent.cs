using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class MapModeRestrictionComponent : Component {
        public List<BattleMode> AvailableModes { get; set; }
    }
}