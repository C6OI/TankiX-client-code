using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1511432334883L)]
    public class VisualScoreKillEvent : VisualScoreEvent {
        public string TargetUid { get; set; }

        public int TargetRank { get; set; }
    }
}