using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientQuests.API {
    [SerialVersionUID(1516362851363L)]
    public interface BattleQuestTemplate : Template {
        [AutoAdded]
        BattleQuestComponent battleQuest();

        [AutoAdded]
        [PersistentConfig]
        DescriptionItemComponent descriptionItem();

        [AutoAdded]
        [PersistentConfig]
        ImageItemComponent imageItem();

        BattleQuestTargetComponent battleQuestTarget();

        BattleQuestRewardComponent battleQuestReward();
    }
}