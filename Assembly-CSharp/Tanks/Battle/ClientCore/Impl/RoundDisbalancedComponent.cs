using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(3051892485776042754L)]
    public class RoundDisbalancedComponent : Component {
        public TeamColor Loser { get; set; }

        public int InitialDominationTimerSec { get; set; }

        public Date FinishTime { get; set; }
    }
}