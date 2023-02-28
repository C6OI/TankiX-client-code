using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class EarnedGameplayChestScoreComponent : Component {
        public long Earned { get; set; }
    }
}