using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientMapEditor.Impl {
    public class SpawnPointBehaviour : EditorBehavior {
        public BattleMode battleMode;

        public TeamColor teamColor;

        public void Initialize(BattleMode battleMode, TeamColor teamColor) {
            this.battleMode = battleMode;
            this.teamColor = teamColor;
        }
    }
}