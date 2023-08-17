using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1455707646909L)]
    public class CTFConfigComponent : Component {
        public float minDistanceFromMineToBase { get; set; }

        public float enemyFlagActionMinIntervalSec { get; set; }
    }
}