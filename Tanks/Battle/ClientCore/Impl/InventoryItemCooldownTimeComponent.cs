using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1435636582675L)]
    public class InventoryItemCooldownTimeComponent : Component {
        public int CooldownTime { get; set; }

        public Date CooldownStartTime { get; set; }
    }
}