using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1513677547945L)]
    public class BattleRewardGroupComponent : GroupComponent {
        public BattleRewardGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public BattleRewardGroupComponent(long key)
            : base(key) { }
    }
}