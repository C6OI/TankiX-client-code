using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [TemplatePart]
    public interface GameplayChestUserTemplatePart : UserTemplate, Template {
        [AutoAdded]
        EarnedGameplayChestScoreComponent earnedGameplayChestScore();
    }
}