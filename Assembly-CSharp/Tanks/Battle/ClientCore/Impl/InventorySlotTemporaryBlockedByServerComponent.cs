using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636367520290400984L)]
    public class InventorySlotTemporaryBlockedByServerComponent : Component {
        public long BlockTimeMS { get; set; }

        public Date StartBlockTime { get; set; }
    }
}