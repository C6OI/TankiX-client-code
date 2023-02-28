using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493731178976L)]
    public interface ScoreQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        ScoreQuestComponent scoreQuest();
    }
}