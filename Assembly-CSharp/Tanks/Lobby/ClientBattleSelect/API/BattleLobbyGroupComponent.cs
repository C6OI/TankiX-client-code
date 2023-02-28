using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1496052424091L)]
    public class BattleLobbyGroupComponent : GroupComponent {
        public BattleLobbyGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public BattleLobbyGroupComponent(long key)
            : base(key) { }
    }
}