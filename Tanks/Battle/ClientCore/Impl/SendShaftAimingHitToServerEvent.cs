using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SendShaftAimingHitToServerEvent : Event {
        public SendShaftAimingHitToServerEvent() { }

        public SendShaftAimingHitToServerEvent(TargetingData targetingData) => TargetingData = targetingData;

        public TargetingData TargetingData { get; set; }
    }
}