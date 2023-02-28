using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636362351589294849L)]
    public class ApplyEmergencyProtectionHealingEvent : Event {
        public float FixedHealingAmount { get; set; }

        public float RelativeHealingAmount { get; set; }
    }
}