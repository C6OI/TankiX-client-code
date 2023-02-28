using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.API {
    public class FeedbackGraphicsRestrictionsComponent : Component {
        public bool HealthFeedbackAllowed { get; set; }

        public bool SelfTargetHitFeedbackAllowed { get; set; }
    }
}