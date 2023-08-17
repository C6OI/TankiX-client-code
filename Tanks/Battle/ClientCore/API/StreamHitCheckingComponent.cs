using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824352777145226L)]
    public class StreamHitCheckingComponent : Component {
        public float LastSendToServerTime { get; set; }

        public float LastCheckTime { get; set; }

        public HitTarget LastSentTankHit { get; set; }

        public StaticHit LastSentStaticHit { get; set; }
    }
}