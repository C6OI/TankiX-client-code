using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1485231135123L)]
    [Shared]
    public class UnitGroupComponent : GroupComponent {
        public UnitGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public UnitGroupComponent(long key)
            : base(key) { }
    }
}