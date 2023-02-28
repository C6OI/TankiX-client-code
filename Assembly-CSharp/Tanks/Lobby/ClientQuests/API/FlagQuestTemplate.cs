using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493731148268L)]
    public interface FlagQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        FlagQuestComponent flagQuest();
    }
}