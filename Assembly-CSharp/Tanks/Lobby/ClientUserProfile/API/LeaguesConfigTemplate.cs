using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1502790438311L)]
    public interface LeaguesConfigTemplate : Template {
        SeasonEndDateComponent seasonEndDate();

        CurrentSeasonNumberComponent currentSeasonNumber();

        [AutoAdded]
        [PersistentConfig]
        CurrentSeasonNameComponent currentSeasonName();
    }
}