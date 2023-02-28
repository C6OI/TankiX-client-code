using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1493115428832L)]
    public interface OldQuestTemplate : BaseQuestTemplate, ItemImagedTemplate, Template {
        UserRankComponent userRank();

        [AutoAdded]
        [PersistentConfig]
        QuestVariationsComponent questVariations();

        [AutoAdded]
        [PersistentConfig("order")]
        OrderItemComponent orderItem();
    }
}