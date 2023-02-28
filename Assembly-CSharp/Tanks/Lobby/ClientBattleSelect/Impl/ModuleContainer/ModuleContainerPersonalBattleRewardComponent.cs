using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer {
    [Shared]
    [SerialVersionUID(1513929654095L)]
    public class ModuleContainerPersonalBattleRewardComponent : Component {
        public long СontainerId { get; set; }
    }
}