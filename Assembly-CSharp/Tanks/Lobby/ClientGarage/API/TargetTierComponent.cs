using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636462061562673727L)]
    public class TargetTierComponent : Component {
        public int TargetTier { get; set; }

        public int MaxExistTier { get; set; }

        public bool ContainsAllTierItem { get; set; } = true;

        public List<long> ItemList { get; set; } = new();
    }
}