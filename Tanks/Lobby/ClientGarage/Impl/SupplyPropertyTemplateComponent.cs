using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SupplyPropertyTemplateComponent : Component {
        public TemplateDescription Template { get; set; }

        public string ConfigPath { get; set; }
    }
}