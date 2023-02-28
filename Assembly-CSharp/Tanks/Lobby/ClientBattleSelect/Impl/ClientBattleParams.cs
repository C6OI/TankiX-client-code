using Tanks.Battle.ClientCore.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ClientBattleParams {
        public BattleMode BattleMode { get; set; }

        public long MapId { get; set; }

        public int MaxPlayers { get; set; }

        public int TimeLimit { get; set; }

        public int ScoreLimit { get; set; }

        public bool FriendlyFire { get; set; }

        public GravityType Gravity { get; set; }

        public bool KillZoneEnabled { get; set; }

        public bool DisabledModules { get; set; }

        public override string ToString() =>
            string.Format("BattleMode: {0}, MapId: {1}, MaxPlayers: {2}, TimeLimit: {3}, ScoreLimit: {4}, FriendlyFire: {5}, Gravity: {6}, KillZoneEnabled: {7}, DisabledModules: {8}",
                BattleMode, MapId, MaxPlayers, TimeLimit, ScoreLimit, FriendlyFire, Gravity, KillZoneEnabled, DisabledModules);
    }
}