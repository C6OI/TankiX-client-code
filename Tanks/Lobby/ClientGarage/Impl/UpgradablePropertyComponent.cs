using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradablePropertyComponent : Component {
        public float InitialValue { get; set; }

        public float FinalValue { get; set; }
    }
}