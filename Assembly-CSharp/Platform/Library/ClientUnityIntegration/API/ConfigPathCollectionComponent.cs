using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public class ConfigPathCollectionComponent : Component {
        public List<string> Collection { get; set; }

        public List<string> Collection1 { get; set; }

        public List<string> Collection2 { get; set; }

        public List<string> Collection3 { get; set; }

        public List<string> Collection4 { get; set; }
    }
}