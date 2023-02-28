using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493731191898L)]
    public interface SupplyQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template {
        SupplyQuestComponent supplyQuest();
    }
}