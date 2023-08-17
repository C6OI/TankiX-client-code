using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.API {
    public class GameTankSettingsComponent : Component {
        public bool MovementControlsInverted { get; set; }
    }
}