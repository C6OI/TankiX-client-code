using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493731205127L)]
    public interface WinQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        WinQuestComponent winQuest();
    }
}