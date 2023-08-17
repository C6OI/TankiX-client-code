namespace Tanks.Lobby.ClientBattleSelect.API {
    public struct BattleEntry {
        public long Id { get; set; }

        public double Relevance { get; set; }

        public int FriendsInBattle { get; set; }

        public long Server { get; set; }

        public long LobbyServer { get; set; }

        public override string ToString() =>
            string.Format("[BattleEntry Id={0} Relevance={1} FriendsInBattle={2} Server={3} Server={4}]",
                Id,
                Relevance,
                FriendsInBattle,
                Server,
                LobbyServer);
    }
}