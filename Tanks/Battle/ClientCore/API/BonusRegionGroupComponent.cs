using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(8566120830355322079L)]
    public class BonusRegionGroupComponent : GroupComponent {
        public BonusRegionGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public BonusRegionGroupComponent(long key)
            : base(key) { }
    }
}