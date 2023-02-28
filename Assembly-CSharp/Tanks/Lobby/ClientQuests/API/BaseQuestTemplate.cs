using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientQuests.Impl;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1474536285473L)]
    public interface BaseQuestTemplate : ItemImagedTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        QuestConditionDescriptionComponent questConditionDescription();

        QuestProgressComponent questProgress();
    }
}