using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SendShotToServerEvent : Event {
        public SendShotToServerEvent() { }

        public SendShotToServerEvent(TargetingData targetingData) => TargetingData = targetingData;

        public TargetingData TargetingData { get; set; }
    }
}