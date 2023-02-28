using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CheckModuleForUpgradeEvent : Event {
        public long StepId { get; set; }

        public long ItemId { get; set; }

        public bool UpgradeAvailable { get; set; }
    }
}