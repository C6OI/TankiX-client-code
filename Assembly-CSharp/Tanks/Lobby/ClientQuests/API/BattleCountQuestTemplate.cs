using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493731132842L)]
    public interface BattleCountQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        BattleCountQuestComponent battleCountQuest();
    }
}