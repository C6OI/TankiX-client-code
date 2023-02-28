using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1507120787784L)]
    public class SquadGroupComponent : GroupComponent {
        public SquadGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public SquadGroupComponent(long key)
            : base(key) { }
    }
}