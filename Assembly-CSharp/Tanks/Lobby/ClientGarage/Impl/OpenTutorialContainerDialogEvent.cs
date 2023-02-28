using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class OpenTutorialContainerDialogEvent : Event {
        public long StepId { get; set; }

        public long ItemId { get; set; }

        public int ItemsCount { get; set; }

        public TutorialContainerDialog dialog { get; set; }
    }
}