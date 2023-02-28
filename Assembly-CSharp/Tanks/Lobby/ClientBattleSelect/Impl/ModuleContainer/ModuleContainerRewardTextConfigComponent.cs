using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer {
    [SerialVersionUID(1513928736665L)]
    public class ModuleContainerRewardTextConfigComponent : Component {
        public string OpenText { get; set; }

        public string WinText { get; set; }

        public string LooseText { get; set; }
    }
}