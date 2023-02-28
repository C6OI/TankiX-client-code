using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1486635434064L)]
    public class InventoryCooldownStateComponent : Component {
        public int CooldownTime { get; set; }

        public Date CooldownStartTime { get; set; }
    }
}