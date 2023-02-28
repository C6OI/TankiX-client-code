using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-9076289125000703482L)]
    public class MapGroupComponent : GroupComponent {
        public MapGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public MapGroupComponent(long key)
            : base(key) { }
    }
}