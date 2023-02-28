using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public abstract class AbstractRestrictionComponent : Component {
        public int RestrictionValue { get; set; }
    }
}