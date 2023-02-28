using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CheckMountedModuleEvent : Event {
        public long StepId { get; set; }

        public long ItemId { get; set; }

        public Slot MountSlot { get; set; }

        public bool ModuleMounted { get; set; }
    }
}