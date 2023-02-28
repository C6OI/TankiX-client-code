using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleAssembleNotEnouthCardWindowComponent : Component {
        public ModuleAssembleNotEnouthCardWindowComponent(int tier) => Tier = tier;

        public int Tier { get; }
    }
}