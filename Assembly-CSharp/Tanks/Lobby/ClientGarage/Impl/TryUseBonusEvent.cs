using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TryUseBonusEvent : Event {
        public long AvailableBonusEnergy { get; set; }
    }
}