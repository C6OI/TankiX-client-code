using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1516789840617L)]
    public class BattleQuestTargetComponent : Component {
        public int TargetValue { get; set; }
    }
}