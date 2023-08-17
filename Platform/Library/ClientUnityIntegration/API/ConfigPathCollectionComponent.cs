using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public class ConfigPathCollectionComponent : Component {
        public List<string> Collection { get; set; }
    }
}