using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1452687129483L)]
    public interface RanksExperiencesConfigTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        RanksExperiencesConfigComponent ranksExperiencesConfig();
    }
}