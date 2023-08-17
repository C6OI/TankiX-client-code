using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1436532217083L)]
    [Shared]
    public class BattleScoreComponent : Component {
        public int Score { get; set; }

        public int ScoreRed { get; set; }

        public int ScoreBlue { get; set; }
    }
}