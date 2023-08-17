using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientHome.API {
    [SerialVersionUID(635750683841688820L)]
    public interface HomeScreenTemplate : Template {
        [State]
        [PersistentConfig]
        HomeScreenLocalizedStringsComponent homeScreenLocalizedStrings();
    }
}