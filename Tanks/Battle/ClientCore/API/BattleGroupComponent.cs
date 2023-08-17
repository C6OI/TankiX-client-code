using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1140613249019529884L)]
    [Shared]
    public class BattleGroupComponent : GroupComponent {
        public BattleGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public BattleGroupComponent(long key)
            : base(key) { }
    }
}