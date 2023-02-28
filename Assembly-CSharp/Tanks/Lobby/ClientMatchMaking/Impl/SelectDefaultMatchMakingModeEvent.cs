using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class SelectDefaultMatchMakingModeEvent : Event {
        public Optional<Entity> DefaultMode { get; set; }
    }
}