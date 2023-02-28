namespace Tanks.Lobby.ClientBattleSelect.API {
    public class PersonalBattleInfo {
        public bool InLevelRange { get; set; }

        public bool CanEnter { get; set; }

        public override string ToString() => string.Format("InLevelRange: {0}, CanEnter: {1}", InLevelRange, CanEnter);
    }
}