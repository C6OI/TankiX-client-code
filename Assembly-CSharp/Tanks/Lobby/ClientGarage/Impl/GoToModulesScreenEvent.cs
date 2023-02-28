using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoToModulesScreenEvent : Event {
        public GoToModulesScreenEvent(TankPartModuleType tankPartModuleType) => TankPartModuleType = tankPartModuleType;

        public TankPartModuleType TankPartModuleType { get; }
    }
}