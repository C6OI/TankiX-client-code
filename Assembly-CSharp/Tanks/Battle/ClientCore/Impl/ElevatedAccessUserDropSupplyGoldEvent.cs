using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636403856821541392L)]
    public class ElevatedAccessUserDropSupplyGoldEvent : Event {
        public GoldType GoldType { get; set; }
    }
}