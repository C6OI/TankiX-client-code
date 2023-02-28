using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class OpenTutorialContainerEvent : Event {
        public long StepId { get; set; }

        public long ItemId { get; set; }
    }
}