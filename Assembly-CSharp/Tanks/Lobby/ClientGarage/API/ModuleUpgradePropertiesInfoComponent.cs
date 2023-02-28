using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636350324450703561L)]
    public class ModuleUpgradePropertiesInfoComponent : Component {
        public TemplateDescription Template { get; set; }

        public string Path { get; set; }
    }
}