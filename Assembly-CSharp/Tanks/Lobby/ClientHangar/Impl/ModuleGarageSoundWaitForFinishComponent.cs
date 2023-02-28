using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ModuleGarageSoundWaitForFinishComponent : Component {
        public ModuleGarageSoundWaitForFinishComponent(ScheduleManager scheduledEvent) => ScheduledEvent = scheduledEvent;

        public ScheduleManager ScheduledEvent { get; set; }
    }
}