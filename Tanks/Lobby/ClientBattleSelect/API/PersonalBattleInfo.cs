namespace Tanks.Lobby.ClientBattleSelect.API {
    public class PersonalBattleInfo {
        public bool IsInLevelRange { get; set; }

        public bool CanEnter { get; set; }

        public override string ToString() => string.Format("IsInLevelRange: {0}, CanEnter: {1}", IsInLevelRange, CanEnter);
    }
}