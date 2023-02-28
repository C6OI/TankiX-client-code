using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    public class PlayAgainEvent : Event {
        public bool ModeIsAvailable { get; set; }

        public Entity MatchMackingMode { get; set; }
    }
}