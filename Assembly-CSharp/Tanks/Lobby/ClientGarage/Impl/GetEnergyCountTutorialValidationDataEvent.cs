using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GetEnergyCountTutorialValidationDataEvent : Event {
        public long Quantums { get; set; }
    }
}