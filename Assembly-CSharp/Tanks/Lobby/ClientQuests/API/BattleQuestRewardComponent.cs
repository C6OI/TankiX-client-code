using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1516873245609L)]
    public class BattleQuestRewardComponent : Component {
        public BattleQuestReward BattleQuestReward { get; set; }

        public int Quantity { get; set; }
    }
}