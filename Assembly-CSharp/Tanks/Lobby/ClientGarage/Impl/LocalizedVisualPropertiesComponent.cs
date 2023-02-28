using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LocalizedVisualPropertiesComponent : Component {
        public Dictionary<string, string> Names { get; set; }

        public Dictionary<string, string> Units { get; set; }
    }
}