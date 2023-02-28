using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1511432353980L)]
    public class VisualScoreAssistEvent : VisualScoreEvent {
        public string TargetUid { get; set; }

        public int Percent { get; set; }
    }
}