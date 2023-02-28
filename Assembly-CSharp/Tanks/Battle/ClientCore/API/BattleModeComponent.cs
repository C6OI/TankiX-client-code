using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1432624073184L)]
    public class BattleModeComponent : Component {
        public BattleMode BattleMode { get; set; }
    }
}