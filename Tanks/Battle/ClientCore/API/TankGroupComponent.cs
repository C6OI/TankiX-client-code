using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(4088029591333632383L)]
    [Shared]
    public class TankGroupComponent : GroupComponent {
        public TankGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public TankGroupComponent(long key)
            : base(key) { }
    }
}