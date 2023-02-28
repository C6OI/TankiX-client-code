using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1502712502830L)]
    public interface LeagueTemplate : Template {
        [AutoAdded]
        LeagueComponent league();

        [AutoAdded]
        [PersistentConfig]
        LeagueIconComponent leagueIcon();

        [AutoAdded]
        [PersistentConfig]
        LeagueNameComponent leagueName();

        [AutoAdded]
        [PersistentConfig]
        LeagueEnergyConfigComponent leagueEnergyConfig();

        TopLeagueComponent topLeague();

        [AutoAdded]
        [PersistentConfig]
        LeagueEnterNotificationTextsComponent leagueEnterNotificationTexts();
    }
}