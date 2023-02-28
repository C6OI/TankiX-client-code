using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class FlavorListComponent : Component {
        public List<string> Collection { get; set; }
    }
}