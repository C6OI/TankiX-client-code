using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CheckModuleForResearchEvent : Event {
        public long StepId { get; set; }

        public long ItemId { get; set; }

        public bool ResearchAvailable { get; set; }
    }
}