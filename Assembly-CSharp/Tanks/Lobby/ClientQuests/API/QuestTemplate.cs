using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493115789575L)]
    public interface QuestTemplate : BaseQuestTemplate, ItemImagedTemplate, Template { }
}