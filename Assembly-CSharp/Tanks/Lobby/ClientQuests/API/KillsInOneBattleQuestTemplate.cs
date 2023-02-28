using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1476072531124L)]
    public interface KillsInOneBattleQuestTemplate : OldQuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        KillsInOneBattleQuestComponent killsInOneBattleQuest();
    }
}