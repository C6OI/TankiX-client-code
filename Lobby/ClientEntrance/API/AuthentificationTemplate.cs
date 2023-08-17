using Lobby.ClientEntrance.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1435135117409L)]
    public interface AuthentificationTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        EntranceValidationRulesComponent entranceValidationRules();
    }
}