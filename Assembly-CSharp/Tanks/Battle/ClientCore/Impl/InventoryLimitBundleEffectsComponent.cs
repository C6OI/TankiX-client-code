using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636378740801778877L)]
    public class InventoryLimitBundleEffectsComponent : Component {
        public int BundleEffectLimit { get; set; }
    }
}