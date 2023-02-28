using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1476708642446L)]
    public interface KillsInOneBattleEveryDayQuestTemplate : OldQuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        KillsInOneBattleEveryDayQuestComponent killsInOneBattleEveryDayQuest();
    }
}