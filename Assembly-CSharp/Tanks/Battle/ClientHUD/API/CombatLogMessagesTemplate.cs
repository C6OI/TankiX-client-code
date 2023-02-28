using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(635719605164895527L)]
    public interface CombatLogMessagesTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        CombatLogCommonMessagesComponent combatLogCommonMessages();

        [AutoAdded]
        [PersistentConfig]
        CombatLogDMMessagesComponent combatLogDMMessages();

        [AutoAdded]
        [PersistentConfig]
        CombatLogTDMMessagesComponent combatLogTDMMessages();

        [AutoAdded]
        [PersistentConfig]
        CombatLogCTFMessagesComponent combatLogCtfMessages();
    }
}