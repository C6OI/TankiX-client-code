using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientMapEditor.Impl {
    public class FlagBehaviour : EditorBehavior {
        public TeamColor teamColor;

        public void SetTeamColor(TeamColor teamColor) {
            this.teamColor = teamColor;
        }
    }
}