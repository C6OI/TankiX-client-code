using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.System {
    public class CalculateTankPartUpgradeCoeffEvent : Event {
        public float UpgradeCoeff { get; set; }
    }
}