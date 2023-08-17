using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class DescriptionItemComponent : Component {
        public string name { get; set; }

        public string description { get; set; }
    }
}