using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1476708526697L)]
    public class KillsInOneBattleEveryDayQuestComponent : Component {
        public int Days { get; set; }
    }
}