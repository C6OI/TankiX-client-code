using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1503469936379L)]
    public class ElevatedAccessUserDropSupplyEvent : Event {
        public BonusType BonusType { get; set; }
    }
}