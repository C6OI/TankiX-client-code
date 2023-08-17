using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientSettings.API {
    public class GraphicsSettingsBuilderComponent : Component {
        public List<Entity> Items { get; set; }
    }
}