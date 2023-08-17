using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class WarmupResourcesComponent : Component {
        public List<string> AssetGuids { get; set; }
    }
}