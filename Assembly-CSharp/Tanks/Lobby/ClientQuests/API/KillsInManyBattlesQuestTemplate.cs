using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1478858847465L)]
    public interface KillsInManyBattlesQuestTemplate : OldQuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        KillsInManyBattlesQuestComponent killsInManyBattlesQuest();
    }
}