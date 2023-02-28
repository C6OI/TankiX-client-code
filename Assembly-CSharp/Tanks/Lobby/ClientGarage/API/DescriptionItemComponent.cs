using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class DescriptionItemComponent : Component {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Flavor { get; set; }
    }
}