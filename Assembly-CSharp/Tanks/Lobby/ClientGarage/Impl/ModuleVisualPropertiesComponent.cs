using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleVisualPropertiesComponent : Component {
        public ModuleVisualPropertiesComponent() => Properties = new List<ModuleVisualProperty>();

        public List<ModuleVisualProperty> Properties { get; set; }
    }
}