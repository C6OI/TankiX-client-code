using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientEntrance.Impl;

namespace Tanks.Lobby.ClientEntrance.API {
    [SerialVersionUID(1435135117409L)]
    public interface AuthentificationTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        EntranceValidationRulesComponent entranceValidationRules();
    }
}