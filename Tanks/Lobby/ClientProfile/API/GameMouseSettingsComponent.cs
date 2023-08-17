using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.API {
    public class GameMouseSettingsComponent : Component {
        public bool MouseControlAllowed { get; set; }

        public bool MouseVerticalInverted { get; set; }

        public float MouseSensivity { get; set; }
    }
}