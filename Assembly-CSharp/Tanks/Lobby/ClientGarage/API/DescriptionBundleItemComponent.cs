using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class DescriptionBundleItemComponent : Component {
        public Dictionary<string, string> Names { get; set; }
    }
}