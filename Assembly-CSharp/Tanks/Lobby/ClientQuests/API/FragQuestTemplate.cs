using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493731166999L)]
    public interface FragQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        FragQuestComponent fragQuest();
    }
}