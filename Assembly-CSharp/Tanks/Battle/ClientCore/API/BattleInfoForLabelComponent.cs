using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1467010512940L)]
    public class BattleInfoForLabelComponent : Component {
        public string BattleMode { get; set; }
    }
}