using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    [Shared]
    [SerialVersionUID(1516709775798L)]
    public class BattleQuestProgressComponent : Component {
        public int CurrentValue { get; set; }
    }
}