using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ItemRarityComponent : Component {
        public ItemRarityType RarityType { get; set; }

        public bool NeedRarityFrame { get; set; }
    }
}