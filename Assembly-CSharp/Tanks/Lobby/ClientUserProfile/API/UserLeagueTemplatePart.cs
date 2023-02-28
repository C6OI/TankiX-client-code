using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [TemplatePart]
    public interface UserLeagueTemplatePart : UserTemplate, Template {
        [AutoAdded]
        UserLeaguePlaceComponent userLeaguePlace();
    }
}