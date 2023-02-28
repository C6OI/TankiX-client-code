using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HideBattleChatMessagesSheduleComponent : Component {
        public HideBattleChatMessagesSheduleComponent(ScheduledEvent scheduledEvent) => ScheduledEvent = scheduledEvent;

        public HideBattleChatMessagesSheduleComponent() { }

        public ScheduledEvent ScheduledEvent { get; set; }
    }
}