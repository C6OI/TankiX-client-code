using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class CardsContainerOpeningSoundWaitForFinishComponent : Component {
        public CardsContainerOpeningSoundWaitForFinishComponent(ScheduleManager scheduledEvent) => ScheduledEvent = scheduledEvent;

        public ScheduleManager ScheduledEvent { get; set; }
    }
}