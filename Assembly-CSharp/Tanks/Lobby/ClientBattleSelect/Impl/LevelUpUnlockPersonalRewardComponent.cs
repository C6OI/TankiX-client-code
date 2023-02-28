using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1514202494334L)]
    public class LevelUpUnlockPersonalRewardComponent : Component {
        public List<Entity> Unlocked { get; set; }
    }
}