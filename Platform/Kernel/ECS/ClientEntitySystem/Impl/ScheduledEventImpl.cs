using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ScheduledEventImpl : ScheduledEvent {
        readonly ScheduleManager scheduleManager;

        public ScheduledEventImpl(Event scheduledEvent, ScheduleManager scheduleManager) =>
            this.scheduleManager = scheduleManager;

        public ScheduleManager Manager() => scheduleManager;
    }
}