using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1432624073184L)]
    [Shared]
    public class BattleModeComponent : Component {
        public BattleMode BattleMode { get; set; }
    }
}