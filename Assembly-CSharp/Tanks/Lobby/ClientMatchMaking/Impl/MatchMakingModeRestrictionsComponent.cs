using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class MatchMakingModeRestrictionsComponent : Component {
        public int MinimalRank { get; set; }

        public int MaximalRank { get; set; } = int.MaxValue;

        public int MinimalShowRank { get; set; }

        public int MaximalShowRank { get; set; } = int.MaxValue;
    }
}