using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.API {
    public class GameTankSettingsComponent : Component {
        public bool MovementControlsInverted { get; set; }

        public bool DamageInfoEnabled { get; set; }

        public bool HealthFeedbackEnabled { get; set; }

        public bool SelfTargetHitFeedbackEnabled { get; set; }
    }
}