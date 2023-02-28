using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [SerialVersionUID(1452687129483L)]
    public interface RanksExperiencesConfigTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        RanksExperiencesConfigComponent ranksExperiencesConfig();
    }
}